import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { QueryObject } from 'app/core/models/query-objects';
import { CityService } from 'app/core/services/city.service';
import { CityFormComponent } from '../city-form/city-form.component';

@Component({
  selector: 'app-cities',
  templateUrl: './city-list.component.html',
  styleUrls: ['./city-list.component.css']
})
export class CityListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'province', 'actions'];

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
  cities: any[] = [];
  allCities: any[] = [];
  showFilter: boolean = false;
  searchText: string = '';
  form: FormGroup = Object.create(null);
  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;
  private subs = new Subscription();
  constructor(
    public cityService: CityService,
    private dialog: MatDialog,
    private alert: AlertService,
    public location: Location,
    private loaderService: LoaderService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateCities();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateCities() {

    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.cityService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.queryResult.items = result.data.items;
          this.allCities = result.data.items;
          this.cities = this.allCities;

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

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateCities();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCities();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(CityFormComponent, {
      width: '30em',
      data: { cityId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCities();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(cityId: number) {
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
          this.cityService.delete(cityId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateCities();
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

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.cities = this.allCities.filter(
        (u) =>
          u.name.includes(this.searchText) ||
          u.province.includes(this.searchText)

      );
    else if (nameText.length > 0)
      this.cities = this.allCities.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.cities = this.allCities;
  }

  onClearFilter() {
    this.form.reset();
    this.populateCities();
  }

  resetControls() {
    this.dialog.closeAll();
  }
}
