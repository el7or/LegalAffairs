import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';

import { IdentityTypeFormComponent } from '../identity-type-form/identity-type-form.component';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { IdentityTypeService } from 'app/core/services/identity-type.service';

@Component({
  selector: 'app-identity-type-list',
  templateUrl: './identity-type-list.component.html',
  styleUrls: ['./identity-type-list.component.css'],
})
export class IdentityTypeListComponent implements OnInit {

  displayedColumns: string[] = ['position', 'name', 'actions'];

  identityTypes: any[] = [];
  allIdentityTypes: any[] = [];

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
    public identityTypeService: IdentityTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateIdentityTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateIdentityTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.identityTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allIdentityTypes = result.data.items;
          this.identityTypes = this.allIdentityTypes;
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
      this.identityTypes = this.allIdentityTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.identityTypes = this.allIdentityTypes.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.identityTypes = this.allIdentityTypes;
  }

   onClearFilter() {
    this.form.reset();
    this.populateIdentityTypes();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateIdentityTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateIdentityTypes();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(IdentityTypeFormComponent, {
      width: '30em',
      data: { identityTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateIdentityTypes();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(identityTypeId: number) {
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
          this.identityTypeService.delete(identityTypeId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateIdentityTypes();
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
