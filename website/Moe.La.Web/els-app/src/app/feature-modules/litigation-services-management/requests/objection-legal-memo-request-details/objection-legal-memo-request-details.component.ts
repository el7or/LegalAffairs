import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { LoaderService } from 'app/core/services/loader.service';
import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Department } from 'app/core/enums/Department';
import { ObjectionLegalMemoRejectFormComponent } from '../objection-legal-memo-reject-form/objection-legal-memo-reject-form.component';

@Component({
  selector: 'app-objection-legal-memo-request-details',
  templateUrl: './objection-legal-memo-request-details.component.html',
  styleUrls: ['./objection-legal-memo-request-details.component.css']
})
export class ObjectionLegalMemoRequestDetailsComponent {

  @Input('request') request: any | undefined;
  private subs = new Subscription();
  viewMore: boolean = false;
  form: FormGroup = Object.create(null);

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
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder
  ) {
  }

  ngOnInit() {

    this.form = this.fb.group({
      replyNote: ['', Validators.compose([Validators.required, Validators.maxLength(400)])],
    });
  }

  ReplyObjectionMemoRequest(RequestStatus: number) {
    if (RequestStatus == this.RequestStatus.Accepted)//Accept Document Request
    {
      this.form.controls['replyNote'].setValidators(null);
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
            this.requestService.replyObjectionLegalMemoRequest({ Id: this.request.request.id, ReplyNote: this.form.get('replyNote').value, RequestStatus: this.RequestStatus.Accepted }).subscribe(
              () => {
                this.loaderService.stopLoading();
                this.alert.succuss('تمت عملية قبول الطلب بنجاح');
                this.router.navigateByUrl('/requests');
              },
              (error) => {
                console.error(error);
                this.loaderService.stopLoading();
                this.alert.error('فشلت عملية قبول الطلب !');
              }
            )
          );
        }
      });
    }
  }

  onReject() {
    const dialogRef = this.matDialog.open(ObjectionLegalMemoRejectFormComponent, {
      width: '30em',
      data: {
        requestId: this.request.request.id,
      }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            this.loaderService.stopLoading();
            this.alert.succuss('تمت عملية رفض الطلب بنجاح');
            this.router.navigateByUrl('requests');
          }
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية رفض الطلب !');
        }
      )
    );
  }

}
