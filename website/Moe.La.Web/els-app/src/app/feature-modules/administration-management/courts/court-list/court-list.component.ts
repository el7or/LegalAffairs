import { Location } from '@angular/common';
import {
  Component,
  OnInit,
  ViewChild,
  TemplateRef,
  OnDestroy,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { EnumValue } from 'app/core/models/enum-value';
import { CourtQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { CourtService } from 'app/core/services/court.service';
import { CourFormComponent } from '../court-form/court-form.component';

@Component({
  selector: 'app-court-list',
  templateUrl: './court-list.component.html',
  styleUrls: ['./court-list.component.css'],
})

export class CourtListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'courtCategory',
    'type',
    'actions',
  ];

  courts: any[] = [];
  allCourts: any[] = [];
  courtTypes: EnumValue[] = [];
  courtCategories: EnumValue[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: CourtQueryObject = {
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
    public courtService: CourtService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private location: Location,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      //name: [],
      courtCategory: [""],
      litigationType: [""],
    });
    this.populateCourts();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateCourts() {
    this.queryObject.litigationType = this.form.value.litigationType;
    this.queryObject.courtCategory = this.form.value.courtCategory;

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.courtService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allCourts = result.data.items;
          this.courts = this.allCourts;
          this.loaderService.stopLoading();
          this.populateCourtTypes();
          this.populateCourtCategories();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateCourtTypes() {
    this.subs.add(
      this.courtService.getCourtTypes().subscribe(
        (result: any) => {
          this.courtTypes = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateCourtCategories() {
    this.subs.add(
      this.courtService.getCourtCategories().subscribe(
        (result: any) => {
          this.courtCategories = result;
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
    if (this.searchText.length > 0)
      this.courts = this.allCourts.filter((item) =>
        item.name.includes(this.searchText) ||
        item.courtCategory.includes(this.searchText) ||
        item.litigationType.includes(this.searchText)
      );
    else this.courts = this.allCourts;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateCourts();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCourts();
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(CourFormComponent, {
      width: '30em',
      data: { courtId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCourts();
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
          this.courtService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateCourts();
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

  onClearFilter() {
    this.form.reset();
    this.populateCourts();
  }

  resetControls() {
    this.dialog.closeAll();
  }
}
