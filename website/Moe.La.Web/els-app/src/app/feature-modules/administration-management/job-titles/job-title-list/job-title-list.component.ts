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
import { JobTitleService } from 'app/core/services/job-title.service';
import { JobTitleFormComponent } from '../job-title-form/job-title-form.component';

@Component({
  selector: 'app-job-title-list',
  templateUrl: './job-title-list.component.html',
  styleUrls: ['./job-title-list.component.css']
})
export class JobTitleListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

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
  jobtitles: any[] = [];
  allJobtitles: any[] = [];
  showFilter: boolean = false;

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;
  private subs = new Subscription();
  searchText: string = '';
  form: FormGroup = Object.create(null);

  constructor(
    public jobTitleService: JobTitleService,
    private dialog: MatDialog,
    private alert: AlertService,
    public location: Location,
    private loaderService: LoaderService,
    private fb: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [''],
    });
    this.populateJobTitles();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateJobTitles() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.jobTitleService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.queryResult.items = result.data.items;
          this.allJobtitles = result.data.items;
          this.jobtitles = this.allJobtitles;
          this.loaderService.stopLoading();
        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateJobTitles();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateJobTitles();
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(JobTitleFormComponent, {
      width: '30em',
      data: { jobTitleId: id },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateJobTitles();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(jobTitleId: number) {
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
          this.jobTitleService.delete(jobTitleId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateJobTitles();
              this.resetControls();
            },
            (error) => {
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
              console.error(error);
            }
          )
        );
      }
    });
  }

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.jobtitles = this.allJobtitles.filter(
        (u) =>
          u.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.jobtitles = this.allJobtitles.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.jobtitles = this.allJobtitles;
  }

  onClearFilter() {
    this.form.reset();
    this.populateJobTitles();
  }

  resetControls() {
    this.dialog.closeAll();
  }
}
