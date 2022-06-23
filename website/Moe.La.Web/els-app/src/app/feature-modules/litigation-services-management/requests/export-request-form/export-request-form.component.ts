import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { RequestTypes } from 'app/core/enums/RequestTypes';

@Component({
  selector: 'app-export-request-form',
  templateUrl: './export-request-form.component.html',
  styleUrls: ['./export-request-form.component.css']
})
export class ExportRequestFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  requestStatus: any;
  requestType: RequestTypes;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExportRequestFormComponent>,
    private alert: AlertService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    private hijriConverter: HijriConverterService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.requestStatus) this.requestStatus = this.data.requestStatus;
    if (this.data.requestType) this.requestType = this.data.requestType;

  }

  ngOnInit(): void {
    this.init();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [this.requestId],
      //requestId: [this.requestId],
      requestStatus: [this.requestStatus],
      transactionNumberInAdministrativeCommunications: [null, Validators.compose([Validators.required])],
      transactionDateInAdministrativeCommunications: [null, Validators.compose([Validators.required])]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.value.transactionDateInAdministrativeCommunications = this.hijriConverter.calendarDateToDate(
      this.form.get('transactionDateInAdministrativeCommunications')?.value?.calendarStart);

    if (this.requestType == RequestTypes.RequestSupportingDocuments) {
      this.subs.add(
        this.requestService.replyDocumentRequest(this.form.value).subscribe(result => {
          this.alert.succuss("تمت تصدير الطلب بنجاح");
          this.loaderService.stopLoading();
          this.onCancel(result);
          this.router.navigateByUrl('/requests')
        }, error => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية تصدير الطلب");
          this.onCancel();
        }));
    }
    else if (this.requestType == RequestTypes.RequestExportCaseJudgment) {
      this.subs.add(
        this.requestService.replyExportCaseJudgmentRequest(this.form.value).subscribe(result => {
          this.alert.succuss("تمت تصدير الطلب بنجاح");
          this.loaderService.stopLoading();
          this.onCancel(result);
          this.router.navigateByUrl('/requests')
        }, error => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية تصدير الطلب");
          this.onCancel();
        }));
    }
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

}
