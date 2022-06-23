import { Location } from '@angular/common';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { AttachmentTypeFormComponent } from '../attachment-type-form/attachment-type-form.component';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { AttachmentTypeService } from 'app/core/services/attachment-type.service';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';

@Component({
  selector: 'app-attachment-type-list',
  templateUrl: './attachment-type-list.component.html',
  styleUrls: ['./attachment-type-list.component.css']
})
export class AttachmentTypeListComponent implements OnInit {

  displayedColumns: string[] = ['position', 'name', 'groupName', 'actions'];

  attachmentTypes: any[] = [];
  allAttachmentTypes: any[] = [];

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
    private attachmentTypeService: AttachmentTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private location: Location,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateAttachmentTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateAttachmentTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.attachmentTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allAttachmentTypes = result.data.items;
          this.attachmentTypes = this.allAttachmentTypes;
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
      this.attachmentTypes = this.allAttachmentTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.attachmentTypes = this.allAttachmentTypes.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.attachmentTypes = this.allAttachmentTypes;
  }

  onClearFilter() {
    this.form.reset();
    this.populateAttachmentTypes();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateAttachmentTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateAttachmentTypes();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(AttachmentTypeFormComponent, {
      width: '30em',
      data: { attachmentTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateAttachmentTypes();
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
          this.attachmentTypeService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateAttachmentTypes();
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
