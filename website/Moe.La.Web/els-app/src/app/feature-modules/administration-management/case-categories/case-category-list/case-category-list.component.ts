import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  AfterViewInit,
  ChangeDetectorRef,
  TemplateRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { CaseCategory } from 'app/core/models/case-category';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { CaseCategoryFormComponent } from '../case-category-form/case-category-form.component';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service'
import { MatSlideToggleChange } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-case-category-list-component',
  templateUrl: './case-category-list.component.html',
  styleUrls: ['./case-category-list.component.css']
})
export class CaseCategoryListComponent implements OnInit ,AfterViewInit, OnDestroy{
  displayedColumns: string[] = ['position','caseSource','mainCategory','firstSubCategory','secondSubCategory','isActive','actions'];

  categories: any[] = [];
  allCategories: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: QueryObject = new QueryObject({
    sortBy: 'name',
    pageSize: 9999,
  });

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;

  private subs = new Subscription();

  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup = Object.create(null);
  forms:any;

  constructor(
    public CaseCategoryService: SecondSubCategoryService,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateCaseCategory();
  }

  // ngAfterViewInit() {
  //   this.cdr.detectChanges();
  //   this.subs.add(
  //     this.sort.sortChange.subscribe(
  //       (result: any) => {
  //         this.queryObject.sortBy = result.active;
  //         this.queryObject.isSortAscending = result.direction == 'asc';
  //         this.populateCaseCategory();
  //       }
  //     )
  //   );
  // }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateCaseCategory() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.CaseCategoryService
        .getWithQuery(this.queryObject)
        .subscribe(
          (result: any) => {
            // this.forms = this.fb.array(
            //   result.data.items.map(e => this.fb.control(e.isActive))
            // );
            this.queryResult.totalItems = result.data.totalItems;
            this.allCategories = result.data.items;
            this.categories=this.allCategories;
            this.loaderService.stopLoading();
          },
          (error:any) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
    );
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(CaseCategoryFormComponent, {
      width: '30em',
      data: { caseCategoryId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCaseCategory();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(departmentId: number) {
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
          this.CaseCategoryService.delete(departmentId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateCaseCategory();
              this.resetControls();
            },
            (error:any) => {
              console.error(error);
              this.loaderService.stopLoading();
              // this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

  changeCategoryActivity(category: any) {
    let textMessage: string = '';
    let sucecssMessage: string = '';
    let errorMessage: string = '';
    category.isActive = !category.isActive;
    if (category.isActive) {
      textMessage = 'هل تريد  تفعيل هذا التصنيف؟';
      sucecssMessage = 'تم  تفعيل التصنيف بنجاح';
      errorMessage = 'فشلت  عملية التصنيف';
    }
    else {
      textMessage = ' هل  تريد  تعطيل هذا التصنيف؟';
      sucecssMessage = 'تم  تعطيل التصنيف بنجاح';
      errorMessage = 'فشلت  عملية التصنيف';
    }
    if (category.id != 0) {
      Swal.fire({
        title: 'تأكيد',
        text: textMessage,
        icon: !category.isActive ? 'warning' : 'success',
        showCancelButton: true,
        confirmButtonColor: '#ff3d71',
        confirmButtonText: 'تأكيد',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.CaseCategoryService.changeCatergoryActivity(category.id,category.isActive).subscribe(
              () => {
                this.loaderService.stopLoading();
                this.alert.succuss(sucecssMessage);
                this.populateCaseCategory();
              },
              (error) => {
                console.error(error);
                this.loaderService.stopLoading();
                this.alert.error(errorMessage);
              }
            )
          );
        }
        else {
          category.isActive = !category.isActive;
        }
      });
    }
    else {
    }
  }

  onChange(ob: MatSlideToggleChange, caseCategoryId) {
    this.CaseCategoryService.changeCatergoryActivity(caseCategoryId, ob.checked).subscribe((res) => {
      this.populateCaseCategory();
    },
      (error) => {
        this.populateCaseCategory();
        console.error(error);
      });
  }

  resetControls() {
    this.dialog.closeAll();
  }

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.categories = this.allCategories.filter(
        (item) =>
          item.name.includes(this.searchText)
            ||item.mainCategory?.name?.includes(this.searchText)
            ||item.firstSubCategory?.name?.includes(this.searchText)
            ||item.caseSource?.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.categories = this.allCategories.filter(
        (item) =>
          item?.name.includes(nameText)
      );
    else this.categories = this.allCategories;
  }

  onClearFilter() {
    this.form.reset();
    this.populateCaseCategory();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateCaseCategory();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCaseCategory();
  }
  
  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateCaseCategory();
        }
      )
    );
  }
}
