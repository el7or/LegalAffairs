import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import * as moment from 'moment';

import { CaseService } from 'app/core/services/case.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseDetails, CaseListItem } from 'app/core/models/case';
import { RequestService } from 'app/core/services/request.service';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { Department } from 'app/core/enums/Department';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';
import { JudgementResult } from 'app/core/enums/JudgementResult';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { CasePartyDetails } from 'app/core/models/case-party';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { ObjectionPermitRequestFormComponent } from '../../requests/objection-permit-request-form/objection-permit-request-form.component';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { AddNextCaseCheckComponent } from '../add-next-case-check/add-next-case-check.component';


@Component({
  selector: 'app-case-view',
  templateUrl: './case-view.component.html',
  styleUrls: ['./case-view.component.css'],
})
export class CaseViewComponent implements OnInit {
  private subs = new Subscription();
  caseId: number = 0;
  case: CaseDetails = new CaseDetails();
  objectionJudgmentLimitDate = new Date();
  hearings!: MatTableDataSource<any>;
  moamalat!: MatTableDataSource<any>;
  caseRelateds!: MatTableDataSource<CaseListItem>;
  objectionRequestStatus!: RequestStatus;
  isDraft: boolean = false;
  requestId!: number;
  permitRequestId: number;
  caseResearcher: boolean = false;
  isAgainst: boolean = false;
  ObjectionLimitation: string = "";
  caseParties: CasePartyDetails[] = [];

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  isDataEntry: boolean = this.authService.checkRole(AppRole.DataEntry);

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  public get AppRole(): typeof AppRole {
    return AppRole;
  }


  public get RequestTypes(): typeof RequestTypes {
    return RequestTypes;
  }
  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }

  public get JudgementResult(): typeof JudgementResult {
    return JudgementResult;
  }

  public get HearingStatus(): typeof HearingStatus {
    return HearingStatus;
  }

  constructor(
    private caseService: CaseService,
    private activatedRoute: ActivatedRoute,
    private alert: AlertService,
    private loaderService: LoaderService,
    private requestService: RequestService,
    private authService: AuthService,
    private location: Location,
    private dialog: MatDialog,
  ) {
    // this.activatedRoute.paramMap.subscribe((params) => {
    //   var id = params.get('id');
    //   if (id != null) {
    //     this.caseId = +id;
    //   }
    // });
    // this.case = <CaseDetails>{};
  }

  ngOnInit(): void {

    this.activatedRoute.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.caseId = +id;

        this.getCase();
      }
    });
    this.case = <CaseDetails>{};

  }

  getCase() {
    this.loaderService.startLoading(LoaderComponent);
    if (this.caseId) {
      this.getCaseObjectionPermitRequest();
      this.subs.add(
        this.caseService.get(this.caseId).subscribe(
          (res: any) => {
            this.case = res.data;
            if (this.case?.receivingJudgmentDate != null) {
              this.case.receivingJudgmentDate = new Date(this.case?.receivingJudgmentDate);
              //Object.assign(this.objectionJudgmentLimitDate,this.case.receivingJudgmentDate)
              //this.objectionJudgmentLimitDate.(this.case.receivingJudgmentDate);
              this.objectionJudgmentLimitDate = new Date(this.case.receivingJudgmentDate)

              this.objectionJudgmentLimitDate.setDate(this.case.receivingJudgmentDate.getDate() + 30);

              this.ObjectionLimitation = "باقى على مهلة الاعتراض " + this.daysDiffPipe(this.objectionJudgmentLimitDate) + " يوم";
            }

            this.isAgainst = this.case?.caseRule?.judgementResult.id == JudgementResult.Against;
            this.hearings = new MatTableDataSource(this.case.hearings);
            this.moamalat = new MatTableDataSource(this.case.caseMoamalat);
            if (this.case.relatedCaseId) {
              let caseRelatedsList: CaseListItem[] = [];
              caseRelatedsList[0] = this.case.relatedCase;
              this.caseRelateds = new MatTableDataSource(caseRelatedsList);
            }
            this.isCaseResearcher(this.case.researchers)
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت  عملية جلب البيانات');
            this.loaderService.stopLoading();
          }
        )
      );
      this.subs.add(
        this.caseService.getCaseParties(this.caseId).subscribe(
          (result: any) => {
            this.case.caseParties = result.data;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

  daysDiffPipe(firstDate: Date): number {

    var m = moment(Date.now());
    var f = moment(firstDate);
    return f.diff(m, 'days');

  }
  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.printCaseDetails(this.case).subscribe(
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

  onBack(): void {
    this.location.back();
  }

  private getCaseObjectionPermitRequest() {
    this.subs.add(
      this.requestService.getCaseObjectionPermitRequest(this.caseId).subscribe(
        (res: any) => {
          this.permitRequestId = res.data ? res.data.id : 0;
        }, (error => {

        })));
  }

  isCaseResearcher(researchers: any[]) {
    if (researchers.length != 0)
      this.caseResearcher = researchers.find((m) => m.id == this.authService.currentUser?.id) != null;
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
              this.location.back();
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

  openObjectionPermitRequestModal(): void {
    const dialogRef = this.dialog.open(ObjectionPermitRequestFormComponent, {
      width: '30em',
      data: { caseId: this.caseId },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            //
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
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
