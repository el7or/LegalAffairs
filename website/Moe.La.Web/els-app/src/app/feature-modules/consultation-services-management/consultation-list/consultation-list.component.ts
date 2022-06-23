import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';

import { ConsultationStatus } from 'app/core/enums/ConsultationStatus';
import { ConsultationListItem } from 'app/core/models/consultation';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ConsultationQueryObject, UserQueryObject, WorkItemTypeQueryObject } from 'app/core/models/query-objects';
import { UserList } from 'app/core/models/user';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderService } from 'app/core/services/loader.service';
import { DepartmentService } from 'app/core/services/department.service';
import { UserService } from 'app/core/services/user.service';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ConsultationService } from 'app/core/services/consultation.service';

@Component({
  selector: 'app-consultation-list',
  templateUrl: './consultation-list.component.html',
  styleUrls: ['./consultation-list.component.css']
})
export class ConsultationListComponent implements OnInit {

  consultations: any[] = [];
  columnsToDisplay = [
    'position',
    'moamalaNumber',
    'moamalaDate',
    'workItemType',
    'subject',
    'user',//Assigned to
    'department',
    'status',//Consultation Status
    'actions',
  ];

  queryObject: ConsultationQueryObject = new ConsultationQueryObject({
    sortBy: 'subject',
  });

  totalItems!: number;

  private subs = new Subscription();

  showFilter: boolean = false;

  dataSource!: MatTableDataSource<ConsultationListItem>;
  @ViewChild(MatSort) sort!: MatSort;

  searchForm: FormGroup = Object.create(null);

  searchText?: string;
  usersList: UserList[] = [];
  users: UserList[] = [];
  filteredUsers!: UserList[] | undefined;
  workItemTypes: any;
  departments: KeyValuePairs[] = [];
  consultationStatus: any[] = [];

  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }

  constructor(
    private cosultationService: ConsultationService,
    private fb: FormBuilder, private alert: AlertService,
    private loaderService: LoaderService, private cdr: ChangeDetectorRef,
    private userService: UserService,
    public authService: AuthService,
    private departmentService: DepartmentService,
    public workItemTypeService: WorkItemTypeService,
    private hijriConverter: HijriConverterService,
    private router:Router
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.populateConsultations();
    this.populateDepartments();
    this.populateConsultationStatus();
  }
  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateConsultations();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }
  initForm() {
    this.searchForm = this.fb.group({
      departmentId: [""],
      assignedTo: [],
      dateFrom: [],
      dateTo: [],
      workItemTypeId: [""],
      status: [""]
    })
  }
  populateConsultations() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.cosultationService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
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
  onFilter() {
    this.queryObject.assignedTo = this.searchForm.controls['assignedTo'].value?.id;
    this.queryObject.status = this.searchForm.controls['status'].value;
    this.queryObject.departmentId = this.searchForm.controls['departmentId'].value;
    this.queryObject.workItemTypeId = this.searchForm.controls['workItemTypeId'].value;

    this.queryObject.dateFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('dateFrom')?.value?.calendarStart
    );
    this.queryObject.dateTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('dateTo')?.value?.calendarStart
    );
    this.populateConsultations();
  }

  onClearFilter() {
    this.queryObject = new ConsultationQueryObject({
      sortBy: 'subject',
    });
    this.searchForm.reset();
    this.populateConsultations();
  }

  onShowFilter() {
    this.showFilter = !this.showFilter;
  }
  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateConsultations();
  }
  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateConsultations();
  }

  populateDepartments() {
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

  populateWorkItemTypes(departmentId: number) {
    this.subs.add(
      this.workItemTypeService.getWithQuery(new WorkItemTypeQueryObject({
        sortBy: 'id',
        pageSize: 9999,
        isSortAscending: false,
        departmentId: departmentId
      })).subscribe(
        (result: any) => {
          this.workItemTypes = result.data.items;
          this.searchForm.controls['assignedTo'].setValue(null);
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateUsers(workItemType) {
    let userQueryObject = new UserQueryObject({
      sortBy: 'name',
      pageSize: 9999,
      workItemTypeId: workItemType
    });
    this.subs.add(
      this.userService.getWithQuery(userQueryObject).subscribe(
        (result: any) => {
          if (result.isSuccess) {
            this.users = result.data.items;
            this.filteredUsers = this.users = this.users.filter((u: UserList) => {
              return u.roles.some(r => r.name != "DepartmentManager")
            });

          }
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  filterUsers() {
    let filterValue = this.searchForm.get('assignedTo')?.value?.toLowerCase();
    if (filterValue) {
      this.filteredUsers = this.users.filter(user => (user.roles.map(r => r.nameAr).join(' ') + ' / ' + user.firstName + ' ' + user.lastName).toLowerCase().includes(filterValue));
    } else {
      this.filteredUsers = this.users;
    }
  }

  displayFn(user?: UserList): string | undefined {
    return user ? user.roles.map(r => r.nameAr).join(' ') + ' / ' + user.firstName + ' ' + user.lastName : '';
  }

  optionUserName(user: UserList) {
    return user ? user.roles.map(r => r.nameAr).join(' ') + ' / ' + user.firstName + ' ' + user.lastName : null;
  }

  populateConsultationStatus() {
    this.subs.add(
      this.cosultationService.getConsultationStatus().subscribe((res: any) => {
        this.consultationStatus = res;
      }, (error) => {
        this.alert.error("فشلت عملية جلب البيانات.");
        console.error(error);
      })
    )
  }

  onEditConsultation(consultationId: number, workItemTypeName: string) {
    if (workItemTypeName == 'نظام' || workItemTypeName == 'لائحة' || workItemTypeName == 'قرار') {
      this.router.navigate(['/consultation/edit-laws', consultationId]);
    } else {
      this.router.navigate(['/consultation/edit', consultationId]);
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
