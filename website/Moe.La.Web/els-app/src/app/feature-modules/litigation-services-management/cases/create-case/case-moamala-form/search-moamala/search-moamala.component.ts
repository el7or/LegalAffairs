import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { MoamalaStatuses } from 'app/core/enums/MoamalaStatuses';
import { CaseMoamalatDetails } from 'app/core/models/case-moamalat';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { MoamalatListItem } from 'app/core/models/moamalat';
import { MoamalatQueryObject, UserQueryObject } from 'app/core/models/query-objects';
import { AppRole } from 'app/core/models/role';
import { UserList } from 'app/core/models/user';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { DepartmentService } from 'app/core/services/department.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { UserService } from 'app/core/services/user.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-search-moamala',
  templateUrl: './search-moamala.component.html',
  styleUrls: ['./search-moamala.component.css']
})
export class SearchMoamalaComponent implements OnInit {
  displayedColumns: string[] = [
    'unifiedNo',
    'moamalaNumber',
    'createdOn',
    'passDate',
    'subject',
    'confidentialDegree',
    'status',
    'senderDepartment',
    'actions',
  ];

  searchForm: FormGroup = Object.create(null);
  queryObject: MoamalatQueryObject = new MoamalatQueryObject();
  dataSource!: MatTableDataSource<MoamalatListItem>;
  totalItems!: number;
  @ViewChild(MatSort) sort!: MatSort;

  ministryDepartments: KeyValuePairs[] = [];
  departments: KeyValuePairs[] = [];
  usersList: UserList[] = [];

  caseMoamalat: CaseMoamalatDetails[] = [];

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

  constructor(
    private moamalatService: MoamalatService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private ministryDepartmentService: MinistryDepartmentService,
    private userService: UserService,
    private departmentService: DepartmentService,
    public dialogRef: MatDialogRef<SearchMoamalaComponent>,
    private hijriConverter: HijriConverterService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.caseMoamalat) this.caseMoamalat = this.data.caseMoamalat;
  }

  ngOnInit(): void {
    this.init();
    this.populateMoamalat();
    this.populateMinistryDepartments();
    this.populateUsers();
    this.populatedepartments();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
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

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMoamalat()
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

  public existElementSelect(id: any): boolean {
    return this.caseMoamalat.some(item => item.moamalaId === id);
  }

  onSubmit(selectedMoamala: MoamalatListItem) {
    this.onCancel(selectedMoamala);
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
