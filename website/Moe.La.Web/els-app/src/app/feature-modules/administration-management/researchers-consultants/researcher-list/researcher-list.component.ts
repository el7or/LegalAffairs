import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import {
  UserQueryObject,
  ResearcherQueryObject,
} from 'app/core/models/query-objects';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { UserService } from 'app/core/services/user.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ResearcherFormComponent } from '../researcher-form/researcher-form.component';
import { ResearcherConsultant, UserDetails } from 'app/core/models/user';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { ArDayOfWeek } from 'app/shared/pipes/ar-day-of-week.pipe';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-researcher-list',
  templateUrl: './researcher-list.component.html',
  styleUrls: ['./researcher-list.component.css'],
})
export class ResearcherListComponent implements OnInit {
  displayedColumns: string[] = [
    'position',
    'researcher',
    'researcherDepartment',
    'consultant',
    'consultantDepartment',
    'connectDate',
    'actions',
  ];

  consultantList: UserDetails[] = [];
  researcherList: UserDetails[] = [];
  showFilter: boolean = false;
  filterPipe = new ArDayOfWeek();
  researcherQueryObject: ResearcherQueryObject = new ResearcherQueryObject({
    sortBy: 'researcher',
    pageSize: 9999,
    isSameBranch: true
  });

  currentPage: number = 0;
  PAGE_SIZE: number = 20;

  dataSource = new MatTableDataSource<ResearcherConsultant>();

  userQueryObject: UserQueryObject = new UserQueryObject({
    roles: ['LegalConsultant'],
    pageSize: 9999,
    branchId: this.authService.currentUser.Branch
  });

  searchForm: FormGroup = Object.create(null);
  @ViewChild(MatSort) sort!: MatSort;

  private subs = new Subscription();

  constructor(
    public researcherConsultantService: ResearcherConsultantService,
    public userService: UserService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialog: MatDialog,
    public authService: AuthService,
    private cdr: ChangeDetectorRef,
    private datePipe: DatePipe,
  ) { }

  ngOnInit() {

    this.searchForm = this.fb.group({
      searchText: [''],
      researcherId: [""],
      consultantId: [""],
    });

    this.populateResearchersList();
    this.populateConsultantsList();
    this.populateResearchers();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();

    this.subs.add(
      this.sort.sortChange.subscribe((result: any) => {
        this.researcherQueryObject.sortBy = result.active;
        this.researcherQueryObject.isSortAscending = result.direction == 'asc';
        this.populateResearchers();
      })
    );
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateResearchers() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.researcherConsultantService
        .getWithQuery(this.researcherQueryObject)
        .subscribe(
          (result: any) => {
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

  populateConsultantsList() {
    this.userQueryObject.roles = ['LegalConsultant'];
    this.subs.add(
      this.userService.getWithQuery(this.userQueryObject).subscribe(
        (result: any) => {
          this.consultantList = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }

  populateResearchersList() {
    this.userQueryObject.roles = ['LegalResearcher'];
    this.subs.add(
      this.userService.getWithQuery(this.userQueryObject).subscribe(
        (result: any) => {
          this.researcherList = result.data.items;

        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value;
    this.dataSource.filteredData = this.dataSource.data.filter(m =>
      m.consultant?.includes(searchText)
      || m.researcher?.includes(searchText)
      || m.researcherDepartment?.includes(searchText)
      || this.datePipe.transform(m.startDate, 'yyyy-MM-dd')?.toString().includes(searchText)
      || m.startDateHigri.includes(searchText)
    );
  }

  onClearFilter() {
    this.searchForm.controls['consultantId'].setValue("");
    this.searchForm.controls['researcherId'].setValue("");
    this.researcherQueryObject.consultantId = null;
    this.researcherQueryObject.researcherId = null;
    this.populateResearchers();
  }

  applyAdvancedFilter() {
    this.researcherQueryObject.researcherId = this.searchForm.controls[
      'researcherId'
    ].value;
    this.researcherQueryObject.consultantId = this.searchForm.controls[
      'consultantId'
    ].value;
    this.populateResearchers();
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  openModal(researcher: any): void {
    const dialogRef = this.dialog.open(ResearcherFormComponent, {
      width: '30em',
      data: {
        id: researcher.id,
        researcherId: researcher.researcherId,
      },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateResearchers();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
}
