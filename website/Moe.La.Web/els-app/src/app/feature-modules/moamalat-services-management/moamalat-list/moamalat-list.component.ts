import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { AuthService } from 'app/core/services/auth.service';
import { MoamalatListItem } from 'app/core/models/moamalat';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';

import { MoamalatService } from 'app/core/services/moamalat.service';
import { MoamalatQueryObject, UserQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MoamalaStatuses } from 'app/core/enums/MoamalaStatuses';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { DepartmentService } from 'app/core/services/department.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { UserService } from 'app/core/services/user.service';
import { UserList } from 'app/core/models/user';
import { AppRole } from 'app/core/models/role';

@Component({
  selector: 'app-moamalat-list',
  templateUrl: './moamalat-list.component.html',
  styleUrls: ['./moamalat-list.component.css'],
})
export class MoamalatListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'unifiedNo',
    'moamalaNumber',
    'createdOn',
    'passDate',
    'subject',
    'description',
    'confidentialDegree',
    'status',
    'department',
    'actions',
  ];

  totalItems!: number;
  queryObject: MoamalatQueryObject = new MoamalatQueryObject();
  searchText: string = '';
  showFilter: boolean = false;
  searchForm: FormGroup = Object.create(null);

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<MoamalatListItem>;


  //lists
  ministryDepartments: KeyValuePairs[] = [];
  departments: KeyValuePairs[] = [];
  usersList: UserList[] = [];

  private subs = new Subscription();


  public get MoamalaStatuses(): typeof MoamalaStatuses {
    return MoamalaStatuses;
  }
  public get ConfidentialDegrees(): typeof ConfidentialDegrees {
    return ConfidentialDegrees;
  }
  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  isGeneralSupervisor: boolean = this.authService.checkRole(
    AppRole.GeneralSupervisor
  );
  isDistributor: boolean = this.authService.checkRole(
    AppRole.Distributor
  );

  constructor(
    private moamalatService: MoamalatService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private hijriConverter: HijriConverterService,
    private ministryDepartmentService: MinistryDepartmentService,
    private userService: UserService,
    private departmentService: DepartmentService,
  ) { }

  ngOnInit() {
    this.init();
    this.populateMoamalat();
    this.populateMinistryDepartments();
    this.populateUsers();
    this.populatedepartments();
  }

  init() {
    this.searchForm = this.fb.group({
      searchText: [],
      status: [0],
      confidentialDegree: [0],
      senderDepartmentId: [""],
      receiverDepartmentId: [""],
      assignedToId: [""],
      createdOnFrom: [],
      createdOnTo: []
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMoamalat(tableData: any = null) {
    // populate table component data
    if (tableData?.page)
      this.queryObject.page = tableData.page;
    if (tableData?.sortBy)
      this.queryObject.sortBy = tableData.sortBy;
    if (tableData?.isSortAscending)
      this.queryObject.isSortAscending = tableData.isSortAscending == 'true';

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
          this.loaderService.stopLoading();
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }
      )
    );
  }

  populateMinistryDepartments() {
    this.subs.add(
      this.ministryDepartmentService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.ministryDepartments = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populatedepartments() {
    this.subs.add(
      this.departmentService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.departments = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateUsers() {
    let userQueryObject = new UserQueryObject({
      sortBy: 'name',
      pageSize: 9999,
    });
    this.subs.add(
      this.userService.getWithQuery(userQueryObject).subscribe(
        (result: any) => {
          this.usersList = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  onFilter() {
    this.queryObject.searchText = this.searchForm.get('searchText')?.value;
    this.queryObject.status = this.searchForm.get('status')?.value;
    this.queryObject.confidentialDegree = this.searchForm.get('confidentialDegree')?.value;
    this.queryObject.senderDepartmentId = this.searchForm.get('senderDepartmentId')?.value;
    this.queryObject.receiverDepartmentId = this.searchForm.get('receiverDepartmentId')?.value;
    this.queryObject.assignedToId = this.searchForm.get('assignedToId')?.value;

    // convert createdOn from date from hijri to miladi
    this.queryObject.createdOnFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('createdOnFrom')?.value?.calendarStart
    );
    // convert createdOn to date hijri to miladi
    this.queryObject.createdOnTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('createdOnTo')?.value?.calendarStart
    );

    this.populateMoamalat()
  }

  onClearFilter() {
    this.queryObject = new MoamalatQueryObject();
    this.searchForm.reset();
    this.populateMoamalat();
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateMoamalat()
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMoamalat()
  }

  advancedFilter() {
    this.showFilter = !this.showFilter;

    if (!this.showFilter) {

      this.queryObject = new MoamalatQueryObject();
      this.populateMoamalat()
      this.searchForm.reset();
    }
  }
}
