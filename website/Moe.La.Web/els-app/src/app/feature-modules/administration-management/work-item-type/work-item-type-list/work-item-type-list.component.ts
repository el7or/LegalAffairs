import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Location } from '@angular/common';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';

import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { WorkItemTypeFormComponent } from '../work-item-type-form/work-item-type-form.component';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';

@Component({
  selector: 'app-work-item-type-list',
  templateUrl: './work-item-type-list.component.html',
  styleUrls: ['./work-item-type-list.component.css']
})
export class WorkItemTypeListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'roles', 'department', 'actions'];

  workItemTypes: any[] = [];
  allWorkItemTypes: any[] = [];

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
    public workItemTypeService: WorkItemTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private location: Location,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [],
    });
    this.populateWorkItemTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateWorkItemTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.workItemTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allWorkItemTypes = result.data.items;
          this.workItemTypes = this.allWorkItemTypes;
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
    if (this.searchText.length > 0)
      this.workItemTypes = this.allWorkItemTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else this.workItemTypes = this.allWorkItemTypes;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateWorkItemTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateWorkItemTypes();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(WorkItemTypeFormComponent, {
      width: '30em',
      data: { workItemTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateWorkItemTypes();
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
          this.workItemTypeService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateWorkItemTypes();
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
