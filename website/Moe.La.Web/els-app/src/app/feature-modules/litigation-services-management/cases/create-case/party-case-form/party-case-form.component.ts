import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  OnInit,
  Output,
  EventEmitter
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { LoaderService } from 'app/core/services/loader.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PartyTypes } from 'app/core/enums/PartyTypes';
import { CaseParty } from 'app/core/models/case-party';
import { CaseService } from 'app/core/services/case.service';
import { Subscription } from 'rxjs';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AlertService } from 'app/core/services/alert.service';
import { CasePartyDetails, CasePartyModel } from 'app/core/models/case-party';
import { AddPartyComponent } from './add-party/add-party.component';
import { SearchPartyComponent } from './search-party/search-party.component';
import { CaseDetails, MainCaseDetails } from 'app/core/models/case';
import { PartyStatus } from 'app/core/enums/PartyTypes'
import * as _ from "lodash";
import { GovernmentOrganizationService } from 'app/core/services/governmentOrganization.service';
import { organization } from 'app/core/models/organization';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-party-case-form',
  templateUrl: './party-case-form.component.html',
  styleUrls: ['./party-case-form.component.css']
})
export class PartyCaseFormComponent implements OnInit, OnChanges, OnDestroy {

  @Input() readOnly: boolean = false;
  @Input() case: MainCaseDetails;
  @Output() onAddParty = new EventEmitter<number>();

  private subs = new Subscription();
  caseParties: CasePartyDetails[] = [];
  form: FormGroup = Object.create(null);
  parties: CaseParty[] = [];
  selectedParty: CasePartyDetails = null;
  partyStatues: any = [];
  currentStatus: number = 0;

  displayedColumns: string[] = [
    'name',
    'partyType',
    // 'partyStatusName',
    'actions',
  ];

  partiesDataSource!: MatTableDataSource<CasePartyDetails>;

  constructor(private dialog: MatDialog,
    private caseService: CaseService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private fb: FormBuilder,
    public governmentOrganizationService: GovernmentOrganizationService,
  ) {

  }

  public get PartyTypes(): typeof PartyTypes {
    return PartyTypes;
  }

  ngOnInit() {
    this.populatePartiesStatuses();
    this.form = this.fb.group({
      name: [null, Validators.compose([Validators.required])],
      partyStatus: []
    });
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      if (this.case != null) {
        this.caseParties = this.case.caseParties;
        this.partiesDataSource = new MatTableDataSource(this.caseParties);
      }
    }
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateParties() {
    if (this.case.id)  {
      this.subs.add(
        this.caseService.getCaseParties(this.case.id).subscribe(
          (result: any) => {
            this.caseParties = result.data;
            this.partiesDataSource = new MatTableDataSource(result.data);
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  populatePartiesStatuses() {
    this.subs.add(
      this.caseService.getPartyStatuses().subscribe(
        (result: any) => {
          this.partyStatues = result;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onSearchParty() {
    this.subs.add(
      this.dialog.open(SearchPartyComponent, {
        width: '40em',
      }).afterClosed().subscribe(
        (res: any) => {
          if (res) {
            this.selectedParty = res;
            this.addCaseParty();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onAddNewParty() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.dialog.open(AddPartyComponent, {
        width: '40em'
      }).afterClosed().subscribe(
        (result: any) => {
          if (result) {
            this.selectedParty = result.data;
            this.addCaseParty();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  addCaseParty() {
    let existParty = this.caseParties.filter(t => t.partyId === this.selectedParty.id)[0];
    if (existParty == null) {
      this.loaderService.startLoading(LoaderComponent);
      let partyDetails = new CasePartyDetails();
      partyDetails.caseId = this.case.id;
      partyDetails.party = _.cloneDeep(this.selectedParty);
      partyDetails.partyId = this.selectedParty.id;
      partyDetails.partyStatus = this.currentStatus == 0 ? null : this.currentStatus;
      partyDetails.partyStatusName = this.currentStatus != 0 ? this.partyStatues?.find(x => x.value == this.currentStatus)?.nameAr : "";
      this.caseParties.push(partyDetails);
      this.partiesDataSource = new MatTableDataSource(this.caseParties);
      this.subs.add(
        this.caseService.addCaseParty(partyDetails).subscribe(
          () => {
            this.loaderService.stopLoading();
            this.populateParties();
            this.onAddParty.emit(this.caseParties.length);
            this.alert.succuss('تم إضافة الطرف بنجاح');
            this.selectedParty = null;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
    else {
      this.alert.error('الطرف موجود فى القضية');
    }
  }

  onChangePartyStatus(caseParty) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.updateCaseParty(caseParty).subscribe(
        () => {
          this.loaderService.stopLoading();
          this.alert.succuss('تم تعديل صفة الطرف بنجاح');
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية التعديل !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onDelete(caseParty) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.caseService.deleteCaseParty(caseParty.id).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              let existParty = this.caseParties.filter(t => t.partyId === caseParty.partyId)[0];
              const index = this.caseParties.indexOf(existParty, 0);
              if (index > -1) {
                this.caseParties.splice(index, 1);
              }
              this.partiesDataSource = new MatTableDataSource(this.caseParties);
              this.onAddParty.emit(this.caseParties.length);
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

}
