import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatDialog } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { DatePipe } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { AuthService } from 'app/core/services/auth.service';
import { UserList } from 'app/core/models/user';
import { UserFormDialogComponent } from '../user-form-dialog/user-form-dialog.component';
import { UserQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { UserService } from 'app/core/services/user.service';
import { Constants } from 'app/core/constants';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class UserListComponent implements OnInit, AfterViewInit, OnDestroy {

  columnsToDisplay: string[] = [
    'position',
    'identityNumber',
    'name',
    'userName',
    'roleGroup',
    'branch',
    // 'departmentsGroup',
    'enabled',
    'actions',
  ];
  expandedDetail = ['userDetails'];

  expandedIndexes: any[] = [];
  dataSource = new MatTableDataSource<UserList>();
  @ViewChild(MatSort) sort!: MatSort;

  //showFilter: boolean = false;

  searchForm: FormGroup = Object.create(null);

  queryObject: UserQueryObject = new UserQueryObject({
    sortBy: 'name',
    pageSize: 9999,
  });

  PAGE_SIZE: number = 20;
  currentPage: number = 0;
  baseUrl = Constants.BASE_URL + "uploads/user-signatures/";
  private subs = new Subscription();

  constructor(
    public userService: UserService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialog: MatDialog,
    public authService: AuthService,
    private cdr: ChangeDetectorRef,
    private datePipe: DatePipe,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.init();
    this.populateUsers();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateUsers();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  init() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
  }

  populateUsers() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          result.data.items.map(i => i.signature = this.sanitizer.bypassSecurityTrustUrl(i.signature));

          this.dataSource = new MatTableDataSource(result.data.items);
          this.applyFilter();
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

  onChange(ob: MatSlideToggleChange, userId) {
    this.userService.updateEnabledUser(userId, ob.checked).subscribe((res) => {
      this.populateUsers();
    },
      (error) => {
        this.populateUsers();
        console.error(error);
      });
  }

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value.trim().toLowerCase();
    this.dataSource.filteredData = this.dataSource.data.filter(m => m.identityNumber?.includes(searchText)
      || m.firstName.includes(searchText)
      || m.lastName.includes(searchText)
      || (m.firstName + " " + m.lastName).includes(searchText)
      || m.email.toLowerCase().includes(searchText)
      || this.datePipe.transform(m.createdOn, 'yyyy-MM-dd')?.toString().includes(searchText)
      || m.createdOnHigri.includes(searchText)
      || m.roleGroup.includes(searchText)
      || m.branch.includes(searchText)
      || m.departmentsGroup.includes(searchText)
      || m.userName.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  openModal(): void {
    const dialogRef = this.dialog.open(UserFormDialogComponent, {
      width: '80%',
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateUsers();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(userId: number) {
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
          this.userService.delete(userId).subscribe(() => {
            this.populateUsers();
            this.loaderService.stopLoading();
            this.alert.succuss('تمت عملية الحذف  بنجاح');
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

  onClickRow(i: number) {
    if (!this.expandedIndexes.includes(i)) {
      this.expandedIndexes.push(i);

    }
    else {
      this.expandedIndexes.splice(this.expandedIndexes.indexOf(i), 1);

    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
