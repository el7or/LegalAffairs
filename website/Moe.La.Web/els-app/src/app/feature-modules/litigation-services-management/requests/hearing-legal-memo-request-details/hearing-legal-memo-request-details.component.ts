import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { LoaderService } from 'app/core/services/loader.service';
import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { HearingLegalMemoRequestReplyFormComponent } from '../hearing-legal-memo-request-reply-form/hearing-legal-memo-request-reply-form.component';
import { Department } from 'app/core/enums/Department';

@Component({
  selector: 'app-hearing-legal-memo-request-details',
  templateUrl: './hearing-legal-memo-request-details.component.html',
  styleUrls: ['./hearing-legal-memo-request-details.component.css']
})
export class HearingLegalMemoRequestDetailsComponent {

  @Input('request') request: any | undefined;
  private subs = new Subscription();
  viewMore: boolean = false;
  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  isConsultant: boolean = this.authService.checkRole(AppRole.LegalConsultant, Department.Litigation);
  constructor(
    private requestService: RequestService,
    private alert: AlertService,
    private loaderService: LoaderService,
    public authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) {
  }


  ReplyHearingMemoRequest(RequestStatus: number) {
    if (RequestStatus == this.RequestStatus.Accepted)//Accept Document Request
    {
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
            this.requestService.replyAddingMemoHearingRequest({ Id: this.request.request.id, ReplyNote: '', RequestStatus: this.RequestStatus.Accepted }).subscribe(
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
    else {
      this.dialog.open(HearingLegalMemoRequestReplyFormComponent, {
        width: '30em',
        data: { requestId: this.request.request.id, requestStatus: RequestStatus }
      });
    }

  }
}
