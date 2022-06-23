import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { RequestQueryObject } from 'app/core/models/query-objects';
import { ExportRequestFormComponent } from '../export-request-form/export-request-form.component';
import { Department } from 'app/core/enums/Department';
import { RequestList } from 'app/core/models/request';
import { AppRole } from 'app/core/models/role';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderService } from 'app/core/services/loader.service';
import { RequestService } from 'app/core/services/request.service';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css'],
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
export class RequestListComponent implements OnInit, OnDestroy {
  displayedColumns = [
    // 'position',
    'id',
    'requestType',
    'requestStatus',
    'createdOn',
    'lastTransactionDate',
    'createdByFullName',
    'actions',
  ];
  expandedIndexes: any[] = [];

  expandedDetail = ['requestDetails'];


  queryObject = new RequestQueryObject({
    sortBy: 'lastTransactionDate',
    pageSize: 999,
    isSortAscending: false
  });
  searchForm: FormGroup = Object.create(null);
  dataSource: MatTableDataSource<RequestList> = new MatTableDataSource<RequestList>();
  currentPage: number = 0;
  PAGE_SIZE: number = 20;
  @ViewChild(MatSort) sort!: MatSort;

  private subs = new Subscription();
  isResearcherInLitigation = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  isLitigationManagerInLitigation: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);

  isResearcherInConsulting = this.authService.checkRole(AppRole.LegalResearcher, Department.Consulting);
  isLitigationManagerInConsulting: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Consulting);
  selectedCaseNumberInSource: any;
  selectedCaseSource: any;
  printCaseYearInSourceHijri: any;
  printCaseNumberInSource: any;

  isAdministrativeCommunicationSpecialist = this.authService.checkRole(AppRole.AdministrativeCommunicationSpecialist);

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get RequestTypes(): typeof RequestTypes {
    return RequestTypes;
  }
  constructor(
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private requestService: RequestService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private dialog: MatDialog,

  ) { }

  ngOnInit(): void {
    this.init();
    this.populateRequests();
  }

  init() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
  }

  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateRequests();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  onClickRow(i: number, requestType: number, requestId: number) {
    if (!this.expandedIndexes.includes(i)) {
      this.expandedIndexes = [];
      this.expandedIndexes.push(i);
      this.getRequestDetails(requestType, requestId);
    }
    else {
      this.expandedIndexes.splice(this.expandedIndexes.indexOf(i), 1);
      this.expandedIndexes = [];
    }
  }
  requestDetails: any;
  getRequestDetails(requestType: number, requestId: number, isForPrint = false) {
    if (!isForPrint) {
      this.selectedCaseNumberInSource = "";
      this.selectedCaseSource = "";
    }

    var $request;
    if (requestType == RequestTypes.RequestSupportingDocuments) {
      $request = this.requestService.getDocumentRequest(requestId);
    }
    else if (requestType == RequestTypes.RequestAttachedLetter) {
      $request = this.requestService.getAttachedLetterRequest(requestId);
    }
    else if (requestType == RequestTypes.RequestResearcherChange) {
      $request = this.requestService.getChangeResearcherRequest(requestId);
    }
    else if (requestType == RequestTypes.RequestResearcherChangeToHearing) {
      $request = this.requestService.getChangeResearcherToHearingRequest(requestId);
    }
    else if (requestType == RequestTypes.ObjectionPermitRequest) {
      $request = this.requestService.getObjectionPermitRequest(requestId);
    }
    else if (requestType == RequestTypes.RequestAddHearingMemo) {
      $request = this.requestService.getHearingLegalMemoRequest(requestId);
    }
    else if (requestType == RequestTypes.ObjectionLegalMemoRequest) {
      $request = this.requestService.getObjectionLegalMemoRequest(requestId);
    }
    else if (requestType == RequestTypes.RequestExportCaseJudgment) {
      $request = this.requestService.getExportCaseJudgmentRequest(requestId);
    }

    $request.subscribe((result: any) => {
      this.requestDetails = result.data;

      console.log(this.requestDetails);

      if (!isForPrint) this.getSelectedCaseDetails(requestType);
      else this.getPrintCaseDetails(requestType);


    }, (error) => {
      console.error(error);
    });
  }

  getSelectedCaseDetails(requestType: number) {
    if (requestType == RequestTypes.RequestAddHearingMemo) {
      this.selectedCaseNumberInSource = this.requestDetails?.hearing?.case?.caseNumberInSource;
      this.selectedCaseSource = this.requestDetails?.hearing?.case?.caseSource.name;

    }
    else if (requestType == RequestTypes.ObjectionPermitRequest) {
      this.selectedCaseNumberInSource = this.requestDetails?.caseNumberInSource;
      this.selectedCaseSource = this.requestDetails?.caseSource;

    }
    else if (requestType == RequestTypes.ObjectionLegalMemoRequest) {
      this.selectedCaseNumberInSource = this.requestDetails?.case?.caseNumberInSource;
      this.selectedCaseSource = this.requestDetails?.case?.caseSource.name;

    }
    else {
      if (this.requestDetails?.caseNumberInSource) {
        this.selectedCaseNumberInSource = this.requestDetails?.caseNumberInSource;
        this.selectedCaseSource = this.requestDetails?.caseSource;
      }
    }
  }


  getPrintCaseDetails(requestType: number) {

    if (requestType == RequestTypes.RequestAddHearingMemo) {
      this.printCaseNumberInSource = this.requestDetails?.hearing?.case?.caseNumberInSource;
      this.printCaseYearInSourceHijri = this.requestDetails?.hearing?.case?.caseYearInSourceHijri;

    }
    else if (requestType == RequestTypes.ObjectionPermitRequest) {
      this.printCaseNumberInSource = this.requestDetails?.caseNumberInSource;
      this.printCaseYearInSourceHijri = this.requestDetails?.caseYearInSourceHijri;

    }
    else if (requestType == RequestTypes.ObjectionLegalMemoRequest) {
      this.printCaseNumberInSource = this.requestDetails?.case?.caseNumberInSource;
      this.printCaseYearInSourceHijri = this.requestDetails?.case?.caseYearInSourceHijri;

    }
    else {
      if (this.requestDetails?.caseNumberInSource) {
        this.printCaseNumberInSource = this.requestDetails?.caseNumberInSource;
        this.printCaseYearInSourceHijri = this.requestDetails?.caseYearInSourceHijri;
      }
    }
  }


  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateRequests() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.dataSource = new MatTableDataSource(result.data.items);
          this.applyFilter();
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
  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value.trim().toLowerCase()
    this.dataSource.filteredData = this.dataSource.data.filter(m => m.requestType.name.includes(searchText)
      || m.requestStatus.name.includes(searchText)
      || m.createdBy.name.includes(searchText)
      || m.id.toString().includes(searchText)
      || this.datePipe.transform(m.createdOn, 'yyyy-MM-dd')?.toString().includes(searchText)
      || this.datePipe.transform(m.lastTransactionDate, 'yyyy-MM-dd')?.toString().includes(searchText)
      || m.requestType.name.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  // exportRequest(requestId: any) {
  //   let dialogRef = this.dialog.open(ExportRequestFormComponent, {
  //     width: '50em',
  //     data: { requestId: requestId, requestStatus: RequestStatus.Exported }
  //   });
  //   this.subs.add(
  //     dialogRef.afterClosed().subscribe(
  //       (res) => {
  //         if (res) {
  //           this.populateRequests();
  //         }
  //       },
  //       (error) => {
  //         console.error(error);
  //       }
  //     )
  //   );
  // }

  onPrint(request: any) {
    this.getRequestDetails(request.requestType.id, request.id, true);

    this.loaderService.startLoading(LoaderComponent);
    let result$;
    if (request.requestType.id == RequestTypes.RequestSupportingDocuments) {
      result$ = this.requestService.printDocumentRequest(request.id);
    }
    else if (request.requestType.id == RequestTypes.RequestAttachedLetter)
      result$ = this.requestService.printAttachedLetterRequest(request.id);
    else if (request.requestType.id == RequestTypes.RequestExportCaseJudgment) {
      result$ = this.requestService.printExportCaseJudgmentRequest(request.id);
    }
    else {
      result$ = this.requestService.printRequest(request);
    }

    if (result$) {
      this.subs.add(
        result$.subscribe(
          (data: any) => {
            var downloadURL = window.URL.createObjectURL(data);
            var link = document.createElement('a');
            link.href = downloadURL;
            link.target = '_blank';

            link.download = `${request.requestType.name} رقم ${request.id} للقضية ${this.printCaseNumberInSource} لعام ${this.printCaseYearInSourceHijri}`;

            link.click();
            this.loaderService.stopLoading();
          },
          (error: any) => {
            console.error(error);
            this.alert.error('فشل التصدير إلى ملف PDF');
            this.loaderService.stopLoading();
          })
      );
    }
  }
}
