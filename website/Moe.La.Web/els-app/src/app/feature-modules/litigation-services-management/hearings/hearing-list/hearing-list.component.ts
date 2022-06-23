import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { HearingService } from 'app/core/services/hearing.service';
import { HearingListItem } from 'app/core/models/hearing';
import { CourtService } from 'app/core/services/court.service';
import { CaseService } from 'app/core/services/case.service';
import { AppRole } from 'app/core/models/role';
import { AuthService } from 'app/core/services/auth.service';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { HearingQueryObject } from 'app/core/models/query-objects';
import { CaseListItem } from 'app/core/models/case';
import { Department } from 'app/core/enums/Department';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { LegalBoardStatus } from 'app/core/models/legal-board-status';
import { AlertService } from 'app/core/services/alert.service';
import { HearingAssignToFormComponent } from '../hearing-assign-to-form/hearing-assign-to-form.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-hearing-list',
  templateUrl: './hearing-list.component.html',
  styleUrls: ['./hearing-list.component.css'],
})
export class HearingListComponent implements OnInit, OnDestroy {
  columnsToDisplay: string[] = [
    /* 'position', */
    'hearingNumber',
    'hearingDate',
    'hearingTime',
    'caseNumberInSource',
    'court',
    'circleNumber',
    'type',
    'status',
    'actions',
  ];
  expandedDetail = ['hearingDetails'];
  expandedIndexes: any[] = [];

  public legalBoardStatus = LegalBoardStatus;

  queryObject: HearingQueryObject = new HearingQueryObject({ sortBy: 'hearingDate', isSortAscending: false });
  totalItems!: number;

  searchForm: FormGroup = Object.create(null);
  showFilter: boolean = false;
  searchText: string = '';

  dataSource!: MatTableDataSource<HearingListItem>;
  @ViewChild(MatSort) sort!: MatSort;
  filteredCases$: Observable<CaseListItem[]> = new Observable<CaseListItem[]>();

  hearingStatus: any;
  courts: any;
  cases: CaseListItem[] = [];
  selectedCaseId!: number;
  receivingJudgmentDate?: Date = null;
  doneJudgment: boolean = false;
  isForHearingAddition: boolean = false;
  isCaseClosed: boolean;

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }
  private subs = new Subscription();
  isLitigationManager: boolean = this.authService.checkRole(
    AppRole.DepartmentManager, Department.Litigation
  );

  isResearcher: boolean = this.authService.checkRole(
    AppRole.LegalResearcher, Department.Litigation
  );

  constructor(
    private hearingService: HearingService,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private caseService: CaseService,
    private courtService: CourtService,
    private router: ActivatedRoute,
    public authService: AuthService,

  ) {
    this.router.queryParams.subscribe((params) => {
      this.selectedCaseId = params.case;
    });
  }

  ngOnInit() {
    this.initForm();

    this.populateHearings();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateHearings();
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

  initForm() {
    this.searchForm = this.fb.group({
      case: [''],
      caseId: [null],
      courtId: [""],
      status: [""],
      from: [],
      to: [],
    });
  }

  populateHearings() {
    this.loaderService.startLoading(LoaderComponent);
    if (this.selectedCaseId) {
      this.queryObject.caseId = this.selectedCaseId;
    }
    this.subs.add(
      this.hearingService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          const hearingsList: HearingListItem[] = result.data.items;
          this.dataSource = new MatTableDataSource(hearingsList);
          this.populateHearingStatus();
          this.loaderService.stopLoading();
          if (this.selectedCaseId) {
            this.checkReceivingJudgment();
          }
          else {
            this.isForHearingAddition = this.authService.checkRole(
              AppRole.LegalResearcher, Department.Litigation
            );
          }
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateHearingStatus() {
    this.subs.add(
      this.hearingService.getHearingStatus().subscribe(
        (result: any) => {
          this.hearingStatus = result;
          this.populateCourts();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateCourts() {
    this.subs.add(
      this.courtService.getWithQuery({ pageSize: 9999 }).subscribe(
        (result: any) => {
          this.courts = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  onFilter() {
    this.queryObject.courtId = this.searchForm.controls['courtId'].value;
    this.queryObject.status = this.searchForm.controls['status'].value;
    this.queryObject.from = this.searchForm.controls['from'].value;
    this.queryObject.to = this.searchForm.controls['to'].value;

    this.populateHearings();
  }

  onClearFilter() {
    this.queryObject = new HearingQueryObject({
      sortBy: 'name',
    });
    this.searchForm.reset();
    this.populateHearings();
  }

  onShowFilter() {
    this.showFilter = !this.showFilter;
    if (this.cases.length == 0) this.getCasesWithAutoComplete();
  }

  getCasesWithAutoComplete() {
    this.queryObject.pageSize = 9999;
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.cases = result.data.items;
          this.loaderService.stopLoading();

          this.filteredCases$ = this.searchForm.controls[
            'case'
          ].valueChanges.pipe(
            startWith(''),
            map((filterInput) => this._filterCases(filterInput))
          );
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  private _filterCases(filterInput: string): any[] {
    return this.cases.filter((_case: any) =>
      _case.subject?.toLowerCase().includes(filterInput)
    );
  }

  onCaseChanged(caseId: any) {
    this.queryObject.caseId = caseId;
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateHearings();
  }

  resetControls() {
    this.dialog.closeAll();
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.hearingService.onPrint(this.queryObject).subscribe(
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

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateHearings();
  }

  checkReceivingJudgment() {
    if (this.selectedCaseId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.caseService.get(this.selectedCaseId).subscribe(
          (res: any) => {
            if (res.data.researchers.some(researcher => researcher.id === this.authService.currentUser?.id)) {
              this.isForHearingAddition = true;
            }
            this.doneJudgment = res.data.status.id == CaseStatus.DoneJudgment;
            this.isCaseClosed = res.data.status.id == CaseStatus.ClosedCase;
            this.receivingJudgmentDate = res.data.receivingJudgmentDate;
            this.loaderService.stopLoading();
          },
          (error) => {
            this.loaderService.stopLoading();
            console.error(error);
            this.alert.error("فشلت عملية جلب البيانات !");
          }
        )
      );
    }
  }

  displayCaseSubject(Case: any): string {
    return Case && Case.subject ? Case.subject : '';
  }

  onAssign(hearing: HearingListItem) {
    const dialogRef = this.dialog.open(HearingAssignToFormComponent, {
      width: '30em',
      data: {
        hearingId: hearing.id,
        attendantId: hearing.assignedTo?.id,
      },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            hearing.assignedTo = res;
            //this.populateHearings();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(hearingId: number) {
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
          this.hearingService.delete(hearingId).subscribe(res => {
            this.loaderService.stopLoading();
    
            this.alert.succuss('تمت عملية الحذف  بنجاح');
            this.populateHearings();
          }, (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية الحذف !');
          })
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
}
