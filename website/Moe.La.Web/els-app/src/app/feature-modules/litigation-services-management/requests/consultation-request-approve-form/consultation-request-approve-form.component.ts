import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as moment from 'moment';

import { RequestStatus } from 'app/core/enums/RequestStatus';
import { AlertService } from 'app/core/services/alert.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderService } from 'app/core/services/loader.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-consultation-request-approve-form',
  templateUrl: './consultation-request-approve-form.component.html',
  styleUrls: ['./consultation-request-approve-form.component.css']
})
export class ConsultationSupportingDocumentApproveFormComponent implements OnInit {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  requestStatus: any;
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ConsultationSupportingDocumentApproveFormComponent>,
    private alert: AlertService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    private hijriConverter: HijriConverterService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.requestStatus) this.requestStatus = this.RequestStatus.Approved;
  }

  ngOnInit(): void {
    this.init();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  /**
 * Component init.
 */
  private init(): void {
    this.form = this.fb.group({
      requestId: [this.requestId],
      transactionNumberInAdministrativeCommunications: ['', Validators.compose([Validators.required])],
      transactionDateInAdministrativeCommunications: [null, Validators.compose([Validators.required])],
      requestStatus: [8]
    });
  }
  onSubmit() {
    this.form.patchValue({
      transactionDateInAdministrativeCommunications: this.hijriConverter.calendarDateToDate(
        this.form.get('transactionDateInAdministrativeCommunications')?.value?.calendarStart),
      requestStatus: 8
    });
    if ((this.daysDiffPipe(this.form.get('transactionDateInAdministrativeCommunications')?.value)) <= 0) {
      this.loaderService.stopLoading();
      this.alert.error('??????????                     ?????????? ???????????????? ???? ?????????????????? ???????????????? ?????? ???? ???????? ???????? ???? ???? ?????????? ?????????? ??????????.');
      return;
    }
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.replyConsultationSupportingDocument(this.form.value).subscribe(result => {
        this.alert.succuss("?????? ?????????? ???????????? ?????????? ??????????");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/requests')
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("???????? ?????????? ???????????? ??????????");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }
  daysDiffPipe(firstDate: Date): number {

    var m = moment(Date.now());
    return m.diff(firstDate, 'days');

  }
}
