import { Component, OnInit, OnDestroy, ChangeDetectorRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';

import { NotificationTypes } from 'app/core/enums/NotificationTypes';
import { NotificationList } from 'app/core/models/notification-list';
import { NotificationQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderService } from 'app/core/services/loader.service';
import { NotificationService } from 'app/core/services/notification.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'text',
    'createdOn',
    'creationTime',
    // 'actions'
  ];

  dataSource: MatTableDataSource<NotificationList> = new MatTableDataSource<NotificationList>();
  @ViewChild(MatSort) sort!: MatSort;
  queryObject: NotificationQueryObject = new NotificationQueryObject({ pageSize: 999 });
  currentPage: number = 0;
  PAGE_SIZE: number = 999;
  searchForm: FormGroup = Object.create(null);
  private subs = new Subscription();

  public get NotificationTypes(): typeof NotificationTypes {
    return NotificationTypes;
  }
  constructor(
    private notificationService: NotificationService,
    private router: Router,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit() {
    this.init();
    this.populateNotifications();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateNotifications();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  init() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
  }

  populateNotifications() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.notificationService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.dataSource = new MatTableDataSource(result.data.items);
          this.applyFilter();
          this.loaderService.stopLoading();
          this.queryObject = new NotificationQueryObject({ pageSize: 999 });
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  readNotification(notificationId: number, url: string) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(this.notificationService.readNotification(notificationId)
      .subscribe((res: any) => {
        this.alert.succuss("تمت قراءة الاشعار  بنجاح");
        this.router.navigateByUrl(url);
        this.loaderService.stopLoading();
      }, (error) => {
        this.alert.error("فشلت عملية قراءة الاشعار ");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value.trim().toLowerCase()
    this.dataSource.filteredData = this.dataSource.data.filter(m => m.text.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

}

