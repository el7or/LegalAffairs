import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { SubWorkItemTypeQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { SubWorkItemTypeService } from 'app/core/services/sub-work-item-type.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { SubWorkItemTypeFormComponent } from '../sub-work-item-type-form/sub-work-item-type-form.component';

@Component({
  selector: 'app-sub-work-item-type-list',
  templateUrl: './sub-work-item-type-list.component.html',
  styleUrls: ['./sub-work-item-type-list.component.css']
})
export class SubWorkItemTypeListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'workItemType', 'actions'];

  subWorkItemTypes: any[] = [];
  allsubWorkItemTypes: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: SubWorkItemTypeQueryObject = {
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
    public subWorkItemTypeservice: SubWorkItemTypeService,
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
    this.populateSubWorkItemTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateSubWorkItemTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.subWorkItemTypeservice.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allsubWorkItemTypes = result.data.items;
          this.subWorkItemTypes = this.allsubWorkItemTypes;
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
      this.subWorkItemTypes = this.allsubWorkItemTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else this.subWorkItemTypes = this.allsubWorkItemTypes;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateSubWorkItemTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateSubWorkItemTypes();
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(SubWorkItemTypeFormComponent, {
      width: '30em',
      data: { subWorkItemTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateSubWorkItemTypes();
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
          this.subWorkItemTypeservice.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateSubWorkItemTypes();
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
