import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { FieldMissionTypeService } from 'app/core/services/field-mission-type.service';
import { FieldMissionTypeFormComponent } from '../field-mission-type-form/field-mission-type-form.component';

@Component({
  selector: 'app-field-mission-type-list',
  templateUrl: './field-mission-type-list.component.html',
  styleUrls: ['./field-mission-type-list.component.css'],
})
export class FieldMissionTypeListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  fieldMissionTypes: any[] = [];
  allFieldMissionTypes: any[] = [];

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
    public fieldMissionTypeService: FieldMissionTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateFieldMissionTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateFieldMissionTypes() {
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.fieldMissionTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allFieldMissionTypes = result.data.items;
          this.fieldMissionTypes = this.allFieldMissionTypes;
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

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.fieldMissionTypes = this.allFieldMissionTypes.filter(
        (item) =>
        item.name.includes(this.searchText)
      );
      else if (nameText.length > 0)
      this.fieldMissionTypes = this.allFieldMissionTypes.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.fieldMissionTypes = this.allFieldMissionTypes;
  }

  onClearFilter() {
    this.form.reset();
    this.populateFieldMissionTypes();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateFieldMissionTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateFieldMissionTypes();
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(FieldMissionTypeFormComponent, {
      width: '30em',
      data: { fieldMissionTypeId: id ,fieldMissionTypes:this.fieldMissionTypes},
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateFieldMissionTypes();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(fieldMissionTypeId: number) {
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
          this.fieldMissionTypeService.delete(fieldMissionTypeId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateFieldMissionTypes();
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
