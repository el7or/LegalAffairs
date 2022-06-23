import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-export-case-judgment-request-reply-form',
  templateUrl: './export-case-judgment-request-reply-form.component.html',
  styleUrls: ['./export-case-judgment-request-reply-form.component.css']
})
export class ExportCaseJudgmentRequestReplyFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  requestStatus: any;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExportCaseJudgmentRequestReplyFormComponent>,
    private alert: AlertService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.requestStatus) this.requestStatus = this.data.requestStatus;
  }

  ngOnInit(): void {
    this.init();
  }

  /**
* Component init.
*/
  private init(): void {
    this.form = this.fb.group({
      id: [this.requestId],
      replyNote: ['', Validators.compose([Validators.required, Validators.maxLength(400)])],
      requestStatus: [this.requestStatus]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.requestService.replyExportCaseJudgmentRequest(this.form.value).subscribe(result => {
        this.alert.succuss("تمت إعادة الطلب للصياغة بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/requests')
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("فشلت عملية إعادة  الطلب للصياغة");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

}
