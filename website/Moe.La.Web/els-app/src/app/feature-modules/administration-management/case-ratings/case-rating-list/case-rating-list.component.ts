import { CaseRatingFormComponent } from './../case-rating-form/case-rating-form.component';
import { Location } from '@angular/common';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { QueryObject } from '../../../../core/models/query-objects';
import { CaseRatingService } from '../../../../core/services/case-rating.service';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-case-rating-list',
  templateUrl: './case-rating-list.component.html',
  styleUrls: ['./case-rating-list.component.css']
})
export class CaseRatingListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  caseRatings: any[] = [];
  allCaseRatings: any[] = [];

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

  constructor(public caseRatingService: CaseRatingService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    public location: Location,
    private loaderService: LoaderService) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [],
    });
    this.populateCaseRating();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateCaseRating() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseRatingService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allCaseRatings = result.data.items;
          this.caseRatings = this.allCaseRatings;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error)
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  onSearch() {
    if (this.searchText.length > 0)
      this.caseRatings = this.allCaseRatings.filter(
        (item) =>
        item.name.includes(this.searchText)
      );
    else this.caseRatings = this.allCaseRatings;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateCaseRating();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCaseRating();
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(CaseRatingFormComponent, {
      width: '30em',
      data: { caseRatingId: id },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCaseRating();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(caseRatingId: number) {
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
          this.caseRatingService.delete(caseRatingId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateCaseRating();
              this.resetControls();
            },
            (error) => {
              this.loaderService.stopLoading();
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
