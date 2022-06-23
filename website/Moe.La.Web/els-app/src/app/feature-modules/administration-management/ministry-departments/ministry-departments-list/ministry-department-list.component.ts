import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { MinistryDepartmentFormComponent } from '../ministry-departments-form/ministry-departments-form.component';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-ministry-department-list',
  templateUrl: './ministry-department-list.component.html',
  styleUrls: ['./ministry-department-list.component.css']
})
export class MinistryDepartmentListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name' , 'ministrySector', 'actions'];

  ministryDepartments: any[] = [];
  allMinistryDepartments: any[] = [];

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
    public ministryDepartmentService: MinistryDepartmentService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [],
    });
    this.populateMinistryDepartments();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMinistryDepartments() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.ministryDepartmentService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allMinistryDepartments = result.data.items;
          this.ministryDepartments = this.allMinistryDepartments;
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
      this.ministryDepartments = this.allMinistryDepartments.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else this.ministryDepartments = this.allMinistryDepartments;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateMinistryDepartments();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMinistryDepartments();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(MinistryDepartmentFormComponent, {
      width: '30em',
      data: { ministryDepartmentId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateMinistryDepartments();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(ministryDepartmentId: number) {
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
          this.ministryDepartmentService.delete(ministryDepartmentId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateMinistryDepartments();
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
