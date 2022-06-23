import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';

@Component({
  selector: 'app-hearing-legal-memo-request-reply-form',
  templateUrl: './hearing-legal-memo-request-reply-form.component.html',
  styleUrls: ['./hearing-legal-memo-request-reply-form.component.css']
})
export class HearingLegalMemoRequestReplyFormComponent implements OnInit {
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
    private dialogRef: MatDialogRef<HearingLegalMemoRequestReplyFormComponent>,
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
      replyNote: ['', Validators.compose([Validators.required])],
      requestStatus: [this.requestStatus]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.replyAddingMemoHearingRequest(this.form.value).subscribe(result => {
        if (this.requestStatus == this.RequestStatus.Rejected)
          this.alert.succuss("تمت عملية الرفض بنجاح");
        else
          this.alert.succuss("تمت إعادة الطلب للصياغة بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/requests')
      }, error => {
        this.loaderService.stopLoading();
        if (this.requestStatus == this.RequestStatus.Rejected)
          this.alert.error("فشلت عملية الرفض");
        else
          this.alert.error("فشلت عملية إعادة  الطلب للصياغة");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}
