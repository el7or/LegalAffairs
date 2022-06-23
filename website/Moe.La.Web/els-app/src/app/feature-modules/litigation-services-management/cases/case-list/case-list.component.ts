import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';
import {
  trigger,
  style,
  animate,
  state,
  transition
} from '@angular/animations';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { AppRole } from 'app/core/models/role';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { AuthService } from 'app/core/services/auth.service';
import { CaseListItem } from 'app/core/models/case';
import { ChangeCaseStatusFormComponent } from '../change-case-status-form/change-case-status-form.component';
import { SentToBranchManagerFormComponent } from '../send-to-general-manager-form/send-to-general-manager-form.component';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { RequestService } from 'app/core/services/request.service';
import { CaseQueryObject } from 'app/core/models/query-objects';
import { AssignCaseToResearcherComponent } from '../assign-case-to-researcher/assign-case-to-researcher.component';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { CaseSources } from 'app/core/enums/CaseSources';
import { Department } from 'app/core/enums/Department';
import { InitialCaseFormComponent } from '../initial-case-form/initial-case-form.component';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';
import { JudgmentFormComponent } from '../judgment-form/judgment-form.component';
import { JudgementResult } from 'app/core/enums/JudgementResult';
import { AddNextCaseCheckComponent } from '../add-next-case-check/add-next-case-check.component';

@Component({
  selector: 'app-case-list',
  templateUrl: './case-list.component.html',
  styleUrls: ['./case-list.component.css'],
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
export class CaseListComponent implements OnInit, AfterViewInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'id',
    'startDate',
    'caseSource',
    'court',
    'circleNumber',
    'hearingsCount',
    'status',
    'actions',
  ];
  expandedDetail = ['caseDetails'];

  expandedIndexes: any[] = [];

  queryObject: CaseQueryObject = new CaseQueryObject({
    sortBy: 'createdOn',
    isSortAscending: false
  });

  totalItems!: number;

  private subs = new Subscription();

  showFilter: boolean = false;

  cases: CaseListItem[];
  // allowCases: AllowObjectionMemo[];

  dataSource!: MatTableDataSource<CaseListItem>;
  @ViewChild(MatSort) sort!: MatSort;

  searchForm: FormGroup = Object.create(null);
  freeSearchForm: FormGroup = Object.create(null);

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }

  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  public get JudgementResult(): typeof JudgementResult {
    return JudgementResult;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }
  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);
  isRegionsSupervisor: boolean = this.authService.checkRole(AppRole.RegionsSupervisor);
  isBranchManager: boolean = this.authService.checkRole(AppRole.BranchManager);
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  isDataEntry: boolean = this.authService.checkRole(AppRole.DataEntry);

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private caseService: CaseService,
    private requestService: RequestService,
    private dialog: MatDialog,
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
      partyName: []
    });

    this.freeSearchForm = this.fb.group({
      searchText: []
    });
  }

  populateCases() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.cases = result.data.items;
          this.dataSource = new MatTableDataSource(this.cases);
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateCases();
  }

  onReceipt(element: CaseListItem, caseStatus: CaseStatus) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية قبول هذه القضية؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#28a745',
      confirmButtonText: 'قبول',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.caseService.changeStatus(element.id, caseStatus).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.populateCases();
              this.alert.succuss('تم قبول القضية بنجاح');
            },
            (error) => {
              console.error(error);
              this.alert.error(error);
              this.loaderService.stopLoading();
            }
          )
        );
      }
    });
  }

  onChangeCaseStatus(element: any, caseStatus: CaseStatus): void {
    const dialogRef = this.dialog.open(ChangeCaseStatusFormComponent, {
      width: '30em',
      data: { caseId: element.id, caseStatus: caseStatus },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCases();
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  sendToBranchManager(element: any): void {
    const dialogRef = this.dialog.open(SentToBranchManagerFormComponent, {
      width: '30em',
      data: { caseId: element.id },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCases();
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  onChooseResearcher(element: CaseListItem) {
    const dialogRef = this.dialog.open(AssignCaseToResearcherComponent, {
      width: '30em',
      data: { caseId: element.id },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCases();
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }
 

  onAddJudgment(caseId: number) {
    this.dialog.open(JudgmentFormComponent, {
      width: '90%',
      data: { caseId: caseId },
    });
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
      sortBy: 'createdOn',
    });
    this.queryObject.startDateFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateFrom')?.value?.calendarStart
    );
    this.queryObject.startDateTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateTo')?.value?.calendarStart
    );
    this.queryObject.legalStatus = this.searchForm.controls['legalStatus'].value;
    this.queryObject.partyName = this.searchForm.controls['partyName'].value;
    this.populateCases();
  }

  onClearFilter() {
    this.queryObject = new CaseQueryObject({
      sortBy: 'createdOn',
    });
    this.searchForm.reset();
    this.freeSearchForm.reset();
    this.populateCases();
  }

  onSearch() {
    this.queryObject.searchText = this.freeSearchForm.controls['searchText'].value.trim().toLowerCase();
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

  openModal(id: any): void {
    const dialogRef = this.dialog.open(InitialCaseFormComponent, {
      width: '30em',
      //data: { caseCategoryId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          this.populateCases();
        },
        (error) => {
          console.error(error);
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

  onDelete(caseId: number) {
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
          this.caseService.delete(caseId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateCases();
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

  addNextCaseCheckModal(element): void {
    const dialogRef = this.dialog.open(AddNextCaseCheckComponent, {
      width: '50em',
      data: { case: element },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
}
