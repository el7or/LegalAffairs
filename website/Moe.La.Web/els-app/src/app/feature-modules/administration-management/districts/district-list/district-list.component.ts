import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { DistrictService } from 'app/core/services/district.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { DistrictFormComponent } from '../district-form/district-form.component';


@Component({
  selector: 'app-district-list',
  templateUrl: './district-list.component.html',
  styleUrls: ['./district-list.component.css']
})
export class DistrictListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'city', 'actions'];

  districts: any[] = [];
  alldistricts: any[] = [];

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
    public districtService: DistrictService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateDistricts();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateDistricts() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.districtService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.alldistricts = result.data.items;
          this.districts = this.alldistricts;
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
      this.districts = this.alldistricts.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.districts = this.alldistricts.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.districts = this.alldistricts;
  }

  onClearFilter() {
    this.form.reset();
    this.populateDistricts();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateDistricts();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateDistricts();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(DistrictFormComponent, {
      width: '30em',
      data: { districtId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateDistricts();
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
          this.districtService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateDistricts();
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
