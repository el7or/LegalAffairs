import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { InvestigationRecordPartyTypeFormComponent } from 'app/feature-modules/administration-management/investigation-record-party-type/investigation-record-party-type-form/investigation-record-party-type-form.component';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { InvestigationRecordPartyTypeService } from 'app/core/services/investigation-record-party-type.service';

@Component({
  selector: 'app-investigation-record-party-type-list',
  templateUrl: './investigation-record-party-type-list.component.html',
  styleUrls: ['./investigation-record-party-type-list.component.css']
})
export class InvestigationRecordPartyTypeListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  investiationRecordPartyTypes: any[] = [];
  allInvestiationRecordPartyTypes: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;

  private subs = new Subscription();

  constructor(
    public investigateRecordPartyTypeService: InvestigationRecordPartyTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateInvestigateRecordPartyTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateInvestigateRecordPartyTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.investigateRecordPartyTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allInvestiationRecordPartyTypes = result.data.items;
          this.investiationRecordPartyTypes = this.allInvestiationRecordPartyTypes;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.investiationRecordPartyTypes = this.allInvestiationRecordPartyTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.investiationRecordPartyTypes = this.allInvestiationRecordPartyTypes.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.investiationRecordPartyTypes = this.allInvestiationRecordPartyTypes;
  }

  onClearFilter() {
    this.form.reset();
    this.populateInvestigateRecordPartyTypes();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateInvestigateRecordPartyTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateInvestigateRecordPartyTypes();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(InvestigationRecordPartyTypeFormComponent, {
      width: '30em',
      data: { investigationRecordTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateInvestigateRecordPartyTypes();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(investigateRecordPartyTypeId: number) {
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
          this.investigateRecordPartyTypeService.delete(investigateRecordPartyTypeId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateInvestigateRecordPartyTypes();
              this.resetControls();
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

  resetControls() {
    this.dialog.closeAll();
  }

}
