import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';

import { LoaderService } from 'app/core/services/loader.service';
import { CaseListItem } from 'app/core/models/case';
import { CaseService } from 'app/core/services/case.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { CaseSources } from 'app/core/enums/CaseSources';
import { CaseQueryObject } from 'app/core/models/query-objects';

@Component({
  selector: 'app-cases-received-for-pleading-list',
  templateUrl: './cases-received-for-pleading-list.component.html',
  styleUrls: ['./cases-received-for-pleading-list.component.css'],
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
export class CasesReceivedForPleadingComponent
  implements OnInit, AfterViewInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'id',
    'receivedDate',
    'receivedStatus',
    'caseSource',
    'court',
    'circleNumber',
  ];
  // columnsToDisplay = [
  //   'position',
  //   'receivedDate',
  //   'receivedStatus',
  //   'caseSource',
  //   'court',
  //   'circleNumber',
  // ];
  expandedDetail = ['caseDetails'];

  expandedIndexes: any[] = [];
  queryObject: CaseQueryObject = new CaseQueryObject({
    sortBy: 'createdOn',
    isSortAscending: false,
    isManual: false
  });

  totalItems!: number;

  private subs = new Subscription();

  showFilter: boolean = false;

  dataSource!: MatTableDataSource<CaseListItem>;
  @ViewChild(MatSort) sort!: MatSort;

  searchForm: FormGroup = Object.create(null);

  searchText?: string;

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }

  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  isLitigationManager: boolean = this.authService.checkRole(
    AppRole.DepartmentManager, Department.Litigation
  );
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private caseService: CaseService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private hijriConverter: HijriConverterService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.init();
    this.populateCases();
  }
  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateCases();
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
      startDateFrom: [],
      startDateTo: [],
      legalStatus: [""],
      caseSource: [""],
      litigationType: [""],
      receivedStatus: [""],
      isDuplicated: []
    });
  }

  populateCases() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCases();
  }

  isCaseResearcher(researchers: any[]) {
    return researchers.find((m) => m.id == this.authService.currentUser?.id);
  }

  onShowFilter() {
    this.showFilter = !this.showFilter;
    !this.showFilter ? this.onClearFilter() : null;
  }

  onFilter() {
    this.queryObject = new CaseQueryObject({
      sortBy: 'receivedDate',
    });
    this.queryObject.startDateFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateFrom')?.value?.calendarStart
    );
    this.queryObject.startDateTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateTo')?.value?.calendarStart
    );
    this.queryObject.legalStatus = this.searchForm.controls[
      'legalStatus'
    ].value;
    this.queryObject.litigationType = this.searchForm.controls[
      'litigationType'
    ].value;
    this.queryObject.caseSource = this.searchForm.controls[
      'caseSource'
    ].value;
    this.queryObject.receivedStatus = this.searchForm.controls[
      'receivedStatus'
    ].value;
    this.queryObject.isDuplicated = this.searchForm.controls[
      'isDuplicated'
    ].value;
    this.queryObject.isManual = false;
    this.populateCases();
  }

  onClearFilter() {
    this.queryObject = new CaseQueryObject({
      sortBy: 'receivedDate',
      isManual: false
    });
    this.searchForm.reset();
    this.searchText = '';
    this.populateCases();
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateCases();
  }

  onCaseView(id: number) {
    this.router.navigate(['/litigation-services-management/cases/view', id]);
  }

  onExportExcel() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.exportExcelCaseList(this.queryObject).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.download = 'cases-list.xlsx';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل تصدير البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.printCaseList(this.queryObject).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.target = '_blank';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل طباعة البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }
  onClickRow(i: number) {
    if (!this.expandedIndexes.includes(i)) {
      this.expandedIndexes.push(i);

    }
    else {
      this.expandedIndexes.splice(this.expandedIndexes.indexOf(i), 1);

    }
  }
}

enum CaseSource {
  Najiz = '1',
  Moeen = '2',
}
