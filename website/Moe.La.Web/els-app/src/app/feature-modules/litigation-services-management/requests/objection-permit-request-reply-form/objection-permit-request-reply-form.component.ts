import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import Swal from 'sweetalert2';
import { SuggestedOpinon } from 'app/core/enums/SuggestedOpinon';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-objection-permit-request-reply-form',
  templateUrl: './objection-permit-request-reply-form.component.html',
  styleUrls: ['./objection-permit-request-reply-form.component.css']
})
export class ObjectionPermitRequestReplyFormComponent implements OnInit {
  requestId: number;
  caseId: number;
  requestStatus: RequestStatus;
  suggestedOpinon:KeyValuePairs;
  form: FormGroup = Object.create(null);

  private subs = new Subscription();

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get SuggestedOpinon(): typeof SuggestedOpinon {
    return SuggestedOpinon;
  }

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ObjectionPermitRequestReplyFormComponent>,
    private alert: AlertService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.caseId) this.caseId = this.data.caseId;
    if (this.data.requestStatus) this.requestStatus = this.data.requestStatus;
    if (this.data.suggestedOpinon) this.suggestedOpinon = this.data.suggestedOpinon;
  }

  ngOnInit(): void {
    this.init();
  }
  ngDestroy() {
    this.subs.unsubscribe();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [this.requestId, Validators.compose([Validators.required])],
      caseId: [this.caseId, Validators.compose([Validators.required])],
      requestStatus: [this.requestStatus, Validators.compose([Validators.required])],
      replyNote: ['']
    });
  }

  onSubmit() {
    if (this.requestStatus == RequestStatus.Rejected) {
      Swal.fire({
        title: 'تأكيد',
        text: 'هل أنت متأكد من رفض الطلب؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ff3d71',
        confirmButtonText: 'رفض',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.requestService.replyObjectionPermitRequest(this.form.value).subscribe(
              () => {
                this.alert.succuss("تم رفض الطلب بنجاح");
                this.loaderService.stopLoading();
                this.onCancel(result);
                this.router.navigateByUrl('/requests');
              },
              (error) => {
                console.error(error);
                this.loaderService.stopLoading();
                this.alert.error("فشلت عملية رفض الطلب");
              }
            )
          );
        }
      });
    }
    else {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.requestService.replyObjectionPermitRequest(this.form.value).subscribe(
          (result) => {
            this.alert.succuss("تمت الموافقة على الطلب بنجاح");
            this.loaderService.stopLoading();
            this.onCancel(result);
            this.router.navigateByUrl('/requests');
          }, (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error("فشلت عملية الموافقة على الطلب");
            this.onCancel();
          }));
    }
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }
}

