import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import * as moment from 'moment';

import { RequestTypes } from 'app/core/enums/RequestTypes';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AlertService } from 'app/core/services/alert.service';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { AuthService } from 'app/core/services/auth.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-request-view',
  templateUrl: './request-view.component.html',
  styleUrls: ['./request-view.component.css'],
})
export class RequestViewComponent implements OnInit {
  requestId: any;
  requestType: any;
  objectionJudgmentLimitDate: Date = new Date();;
  requestFull: any;
  private subs = new Subscription();

  public get RequestTypes(): typeof RequestTypes {
    return RequestTypes;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  isLitigationManager: boolean = this.authService.checkRole(
    AppRole.DepartmentManager, Department.Litigation
  );
  constructor(
    private requestService: RequestService,
    private activatedRouter: ActivatedRoute,
    private loaderService: LoaderService,
    private authService: AuthService,
    private alert: AlertService
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      this.requestId = params.get('id');
      this.requestType = params.get('type');
    });
  }
  ngDestroy() {
    this.subs.unsubscribe();
  }

  daysDiffPipe(firstDate: Date): number {

    var m = moment(Date.now());
    var f = moment(firstDate);
    return f.diff(m, 'days');

  }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);
    let result$;
    if (this.requestType == RequestTypes.RequestResearcherChange)
      result$ = this.requestService.getChangeResearcherRequest(this.requestId);
    else if (
      this.requestType == RequestTypes.RequestSupportingDocuments ||
      this.requestType == RequestTypes.RequestAttachedLetter
    )
      result$ = this.requestService.getDocumentRequest(this.requestId);
    else if (this.requestType == RequestTypes.RequestAddHearingMemo)
      result$ = this.requestService.getHearingLegalMemoRequest(this.requestId);
    else if (this.requestType == RequestTypes.RequestExportCaseJudgment)
      result$ = this.requestService.getExportCaseJudgmentRequest(this.requestId);
    else if (this.requestType == RequestTypes.ObjectionPermitRequest)
      result$ = this.requestService.getObjectionPermitRequest(this.requestId);
    else if (this.requestType == RequestTypes.ConsultationSupportingDocument)
      result$ = this.requestService.getConsultationSupportingDocument(this.requestId);
    else if (this.requestType == RequestTypes.ObjectionLegalMemoRequest)
      result$ = this.requestService.getObjectionLegalMemoRequest(this.requestId);
    else if (this.requestType == RequestTypes.RequestResearcherChangeToHearing)
      result$ = this.requestService.getChangeResearcherToHearingRequest(this.requestId);

    if (result$)
      result$.subscribe((result: any) => {
        if (result.data) {
          this.requestFull = result.data;
          if (this.requestFull?.case?.receivingJudgmentDate != null) {
            var date = new Date(this.requestFull.case?.receivingJudgmentDate);
            //this.requestFull.case.receivingJudgmentDate = new Date(this.requestFull?.case?.receivingJudgmentDate);
            this.objectionJudgmentLimitDate.setDate(date.getDate() + 30);

          }
        }
        this.loaderService.stopLoading();
      },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل جلب البيانات');
          this.loaderService.stopLoading();
        });
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    let result$;

    if (this.requestType == RequestTypes.RequestSupportingDocuments)
      result$ = this.requestService.printDocumentRequest(this.requestFull.id);
    else if (this.requestType == RequestTypes.RequestAttachedLetter)
      result$ = this.requestService.printAttachedLetterRequest(this.requestFull.id);
    else if (this.requestType == RequestTypes.RequestExportCaseJudgment)
      result$ = this.requestService.printExportCaseJudgmentRequest(this.requestFull.id);

    if (result$) {
      this.subs.add(
        result$.subscribe(
          (data: any) => {
            var downloadURL = window.URL.createObjectURL(data);
            var link = document.createElement('a');
            link.href = downloadURL;
            link.target = '_blank';

            if (this.requestType == RequestTypes.RequestSupportingDocuments || this.requestType == RequestTypes.RequestAttachedLetter){
              link.download = `${this.requestFull.request.requestType.name} رقم ${this.requestFull.id} للقضية ${this.requestFull.caseNumberInSource} لعام ${this.requestFull.caseYearInSourceHijri}`;
            }

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
