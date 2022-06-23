import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { InvestigationRecordParty, InvestigationRecordPartyDetails } from 'app/core/models/investigation-record';
import { AlertService } from 'app/core/services/alert.service';
import { NoorIntegrationService } from 'app/core/services/noor-integration.service';
import { FaresService } from 'app/core/services/fares.service';
import { LoaderService } from 'app/core/services/loader.service';
import { InvestigationRecordPartyTypeService } from 'app/core/services/investigation-record-party-type.service';
import { QueryObject } from 'app/core/models/query-objects';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-parties-form',
  templateUrl: './parties-form.component.html',
  styleUrls: ['./parties-form.component.css']
})
export class PartiesFormComponent implements OnInit {

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  party: InvestigationRecordPartyDetails = new InvestigationRecordPartyDetails();
  partyForSubmit: InvestigationRecordParty = new InvestigationRecordParty();

  editMode: boolean = false;

  ministryDepartments: KeyValuePairs[] = [];
  filteredMinistryDepartments!: KeyValuePairs[] | undefined;
  department: number | undefined;
  searchText: string = '';
  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 9999,
  };
  partyTypes: any[] = [];
  constructor(
    private fb: FormBuilder,
    private noorInterationService: NoorIntegrationService,
    private farisInterationService: FaresService,
    private partyTypeService: InvestigationRecordPartyTypeService,
    private loaderService: LoaderService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<PartiesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data?.party) {
      this.partyForSubmit = this.data?.party;
      this.editMode = true;
    }
  }

  ngOnInit() {
    this.init();

    this.populatePartyTypes();

    if (this.editMode) {
      this.patchForm(this.partyForSubmit)
    }

    ///
    // this.form.controls['identityNumber'].valueChanges.subscribe(
    //   value => {
    //     this.partyForSubmit = new InvestigationRecordParty();
    //     if (this.form.controls['identityNumber'].value.length == 10) { this.onSearch(); }
    //   }
    // );
  }

  getPartyByFaris() {
    this.subs.add(
      this.farisInterationService.getParty(this.searchText, null).subscribe(
        (result: any) => {
          if (result.data) {
            this.party = result.data;
            Object.assign(this.partyForSubmit, this.party);

            this.partyForSubmit.identityNumber = this.form.value.identityNumber;
            this.partyForSubmit.investigationRecordPartyTypeId = this.form.value.partyTypeId;
            this.partyForSubmit.staffTypeName = this.party.staffType.name;
            this.partyForSubmit.staffType = this.party.staffType.id;
            this.partyForSubmit.appointmentStatusName = this.party.appointmentStatus.name;
            this.partyForSubmit.appointmentStatus = this.party.appointmentStatus.id;

            this.form.patchValue({
              identityNumber: this.searchText
            });
          }
        },
        (error) => {
          console.error(error);
          this.party = new InvestigationRecordPartyDetails();
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  getPartyByNoor() {

    this.subs.add(
      this.noorInterationService.getParty(this.searchText, null).subscribe(
        (result: any) => {
          if (result.data) {
            this.party = result.data;
            Object.assign(this.partyForSubmit, this.party);

            this.partyForSubmit.identityNumber = this.form.value.identityNumber;
            this.partyForSubmit.investigationRecordPartyTypeId = this.form.value.partyTypeId;
            this.partyForSubmit.staffTypeName = this.party.staffType.name;
            this.partyForSubmit.staffType = this.party.staffType.id;
            this.partyForSubmit.appointmentStatusName = this.party.appointmentStatus.name;
            this.partyForSubmit.appointmentStatus = this.party.appointmentStatus.id;

            this.form.patchValue({
              identityNumber: this.searchText
            });
            if (this.form.get('identityNumber')?.hasError('notExists'))
              this.form.get('identityNumber')?.setErrors({ notExists: false });
          }
          else {
            //this.form.get('identityNumber')?.setErrors({ notExists: true });
          }
        }, (error) => {
          console.error(error);
          this.party = new InvestigationRecordPartyDetails();
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      ));
  }
  onSearch() {
    this.getPartyByFaris();
    if (this.party.partyName == null || this.party.partyName == "")
      this.getPartyByNoor();

  }
  populatePartyTypes() {
    this.subs.add
      (this.partyTypeService.getWithQuery(this.queryObject).subscribe((res: any) => {
        this.partyTypes = res.data.items;
      }, (error) => {
        this.alert.error("فشلت عملية جلب البيانات");
        console.error(error);
      }))
  }
  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      identityNumber: [null, Validators.compose([Validators.required, Validators.minLength(10), Validators.pattern("^[0-9]*$")])],
      partyTypeId: ["", Validators.compose([Validators.required])]
    });
  }

  patchForm(party: InvestigationRecordParty) {
    this.searchText = party.identityNumber;
    this.form.patchValue({
      identityNumber: party.identityNumber,
      partyTypeId: party.investigationRecordPartyTypeId
    })
  }

  onSubmit() {

    Object.assign(this.partyForSubmit, this.party);

    this.partyForSubmit.identityNumber = this.form.value.identityNumber;
    this.partyForSubmit.investigationRecordPartyTypeId = this.form.value.partyTypeId;
    this.partyForSubmit.staffTypeName = this.party.staffType.name;
    this.partyForSubmit.staffType = this.party.staffType.id;
    this.partyForSubmit.appointmentStatusName = this.party.appointmentStatus.name;
    this.partyForSubmit.appointmentStatus = this.party.appointmentStatus.id;

    this.partyForSubmit.InvestigationRecordPartyTypeName = this.partyTypes.find(t => t.id == this.form.value.partyTypeId)?.name;

    this.onCancel(this.partyForSubmit)

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
