import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';

@Component({
  selector: 'app-consultation-request-reply-form',
  templateUrl: './consultation-request-reply-form.component.html',
  styleUrls: ['./consultation-request-reply-form.component.css']
})
export class ConsultationSupportingDocumentReplyFormComponent implements OnInit {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  requestStatus: any;
  public Editor = CustomEditor;
  public config = {
    language: 'ar'
  };
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ConsultationSupportingDocumentReplyFormComponent>,
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
      requestId: [this.requestId],
      replyNote: ['', Validators.compose([Validators.required, Validators.maxLength(400)])],
      requestStatus: [this.requestStatus]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.replyConsultationSupportingDocument(this.form.value).subscribe(result => {
        if (this.requestStatus == this.RequestStatus.Rejected)
          this.alert.succuss("?????? ?????????? ?????????? ??????????");
        else
          this.alert.succuss("?????? ?????????? ?????????? ?????????????? ??????????");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/requests')
      }, error => {
        this.loaderService.stopLoading();
        if (this.requestStatus == this.RequestStatus.Rejected)
          this.alert.error("???????? ?????????? ??????????");
        else
          this.alert.error("???????? ?????????? ?????????? ?????????? ??????????????");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}
