import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { GovernmentOrganizationFormComponent } from '../governmentOrganization-form/governmentOrganization-form.component';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { GovernmentOrganizationService } from 'app/core/services/governmentOrganization.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-governmentOrganization-list',
  templateUrl: './governmentOrganization-list.component.html',
  styleUrls: ['./governmentOrganization-list.component.css']
})
export class GovernmentOrganizationListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  governmentOrganizations: any[] = [];
  allgovernmentOrganizations: any[] = [];

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
    public governmentOrganizationService: GovernmentOrganizationService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateGovernmentOrganizations();
  } 

  populateGovernmentOrganizations() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.governmentOrganizationService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allgovernmentOrganizations = result.data.items;
          this.governmentOrganizations = this.allgovernmentOrganizations;
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
      this.governmentOrganizations = this.allgovernmentOrganizations.filter(
        (item) =>
        item.name.includes(this.searchText)
      );
      else if (nameText.length > 0)
      this.governmentOrganizations = this.allgovernmentOrganizations.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.governmentOrganizations = this.allgovernmentOrganizations;
  }

  onClearFilter() {
    this.form.reset();
    this.populateGovernmentOrganizations();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateGovernmentOrganizations();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateGovernmentOrganizations();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(GovernmentOrganizationFormComponent, {
      width: '30em',
      data: {governmentOrganizationId: id},
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if(res)
          {
            this.populateGovernmentOrganizations();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(courtId: number) {
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
          this.governmentOrganizationService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateGovernmentOrganizations();
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
  
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
