import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { ConsultationSupportingDocumentReplyFormComponent } from '../consultation-request-reply-form/consultation-request-reply-form.component';
import { ConsultationSupportingDocumentApproveFormComponent } from '../consultation-request-approve-form/consultation-request-approve-form.component';

@Component({
  selector: 'app-consultation-request-details',
  templateUrl: './consultation-request-details.component.html',
  styleUrls: ['./consultation-request-details.component.css'],
})
export class ConsultationSupportingDocumentDetailsComponent {
  @Input('request') request: any | undefined;
  @Input('readOnly') readOnly: boolean = false;
  public Editor = CustomEditor;
  public config = {
    language: 'ar',
    toolbar: 'none',
  };

  form: FormGroup = Object.create(null);

  private subs = new Subscription();
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
    private requestService: RequestService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    public authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) {
  }


  ngDestory() {
    this.subs.unsubscribe();
  }


  ReplyConsultationSupportingDocument(RequestStatus: number) {
    if (RequestStatus == this.RequestStatus.Accepted) {

      Swal.fire({
        title: 'تأكيد',
        text: 'هل أنت متأكد من إتمام عملية قبول الطلب؟',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#00897b',
        confirmButtonText: 'قبول',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.requestService
              .replyConsultationSupportingDocument({
                requestId: this.request.requestId,
                consultationId: this.request.consultationId,
                replyNote: '',
                requestStatus: this.RequestStatus.Accepted
              })
              .subscribe(
                () => {
                  this.loaderService.stopLoading();
                  this.alert.succuss('تمت عملية قبول الطلب بنجاح');
                  this.router.navigateByUrl('/requests');
                },
                (error) => {
                  this.loaderService.stopLoading();
                  console.error(error);
                  this.loaderService.stopLoading();
                  this.alert.error('فشلت عملية قبول الطلب !');
                }
              )
          );
        }
      });
    }
    else if (RequestStatus == this.RequestStatus.Approved) {
      this.dialog.open(ConsultationSupportingDocumentApproveFormComponent, {
        width: '30em',
        data: { requestId: this.request.requestId, consultationId: this.request.consultationId, requestStatus: RequestStatus },
      });
    }
    else {
      this.dialog.open(ConsultationSupportingDocumentReplyFormComponent, {
        width: '30em',
        data: { requestId: this.request.requestId, consultationId: this.request.consultationId, requestStatus: RequestStatus },
      });
    }
  }
}

