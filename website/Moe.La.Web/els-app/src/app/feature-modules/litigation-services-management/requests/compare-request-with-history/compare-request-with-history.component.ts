import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { RequestStatus } from 'app/core/enums/RequestStatus';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestTypes } from 'app/core/enums/RequestTypes';

@Component({
  selector: 'app-compare-request-with-history',
  templateUrl: './compare-request-with-history.component.html',
  styleUrls: ['./compare-request-with-history.component.css']
})
export class CompareRequestWithHistoryFormComponent implements OnInit, OnDestroy {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  historyId: any;
  currentRequest: any;
  requestHistory: any;
  requestType!: RequestTypes;

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get RequestTypes(): typeof RequestTypes {
    return RequestTypes;
  }
  constructor(
    private loaderService: LoaderService,
    private requestService: RequestService,
    private dialogRef: MatDialogRef<CompareRequestWithHistoryFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.historyId) this.historyId = this.data.historyId;
    if (this.data.requestType) this.requestType = this.data.requestType;
  }

  ngOnInit() {
    if (this.data.requestType == this.RequestTypes.RequestSupportingDocuments || this.data.requestType == this.RequestTypes.RequestAttachedLetter)
      this.populateCurrentDocumentRequest();
    if (this.data.requestType == this.RequestTypes.RequestExportCaseJudgment)
      this.populateCurrentExportCaseJudgmentRequest();
  }
  populateCurrentExportCaseJudgmentRequest() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(

      this.requestService.getExportCaseJudgmentRequest(this.requestId)
        .subscribe((result: any) => {
          this.currentRequest = result.data;
          this.loaderService.stopLoading();
          this.populateExportCaseJudgmentRequestHistory();

        }, (error) => {
          console.error(error);
          this.loaderService.stopLoading();
        }));
  }
  populateCurrentDocumentRequest() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(

      this.requestService.getDocumentRequest(this.requestId)
        .subscribe((result: any) => {
          this.currentRequest = result.data;

          this.loaderService.stopLoading();
          this.populateDocumentReuestHistory();

        }, (error) => {
          console.error(error);
          this.loaderService.stopLoading();

        }));
  }
  populateDocumentReuestHistory() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(

      this.requestService.getDocumentRequestHistory(this.historyId)
        .subscribe((result: any) => {
          this.requestHistory = result.data;

          this.loaderService.stopLoading();
        }, (error) => {
          console.error(error);
          this.loaderService.stopLoading();

        }));
  }
  populateExportCaseJudgmentRequestHistory() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.getExportCaseJudgmentRequestHistory(this.historyId)
        .subscribe((result: any) => {

          this.requestHistory = result.data;

          this.loaderService.stopLoading();
        }, (error) => {
          console.error(error);
          this.loaderService.stopLoading();

        }));
  }
  onCancel() {
    this.dialogRef.close();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
