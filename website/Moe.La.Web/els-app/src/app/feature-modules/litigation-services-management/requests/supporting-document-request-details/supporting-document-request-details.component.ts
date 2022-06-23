import { Component, OnInit, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { CaseSupportingDocumentRequestReplyFormComponent } from '../supporting-document-request-reply-form/supporting-document-request-reply-form.component';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { Department } from 'app/core/enums/Department';
import { ExportRequestFormComponent } from '../export-request-form/export-request-form.component';
 
@Component({
  selector: 'app-document-request-details',
  templateUrl: './supporting-document-request-details.component.html',
  styleUrls: ['./supporting-document-request-details.component.css'],
})
export class CaseSupportingDocumentRequestDetailsComponent implements OnInit {
  @Input('request') request: any | undefined;
  @Input('readOnly') readOnly: boolean = false;
    
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
  isConsultant: boolean = this.authService.checkRole(AppRole.LegalConsultant, Department.Litigation);
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);
  isGeneralSupervisor: boolean = this.authService.checkRole(AppRole.GeneralSupervisor);
  content: string;
  constructor(
    private requestService: RequestService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    public authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) {
    // navigate to the same route with different parameter
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    };
  }

  ngOnInit(): void {
    this.initForm();
   }

  ngDestory() {
    this.subs.unsubscribe();
  }

  initForm() {
    this.form = this.fb.group({
      consigneeDepartmentId: [
        this.request.consigneeDepartmentId,
        Validators.compose([Validators.required]),
      ],
    });
  } 

  ReplyDocumentRequest(RequestStatus: number) {
    if (RequestStatus == this.RequestStatus.AcceptedFromConsultant || RequestStatus == this.RequestStatus.AcceptedFromLitigationManager) {
      //Accept Document Request
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
              .replyDocumentRequest({
                Id: this.request.id,
                ReplyNote: '',
                RequestStatus: RequestStatus,
                consigneeDepartmentId: this.form.value
                  .consigneeDepartmentId,
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
      // this.dialog.open(CaseSupportingDocumentRequestApproveFormComponent, {
      //   width: '30em',
      //   data: { requestId: this.request.id, requestStatus: RequestStatus },
      // });

      //approve Document Request
      Swal.fire({
        title: 'تأكيد',
        text: 'هل أنت متأكد من إتمام عملية اعتماد الطلب؟',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#ff3d71',
        confirmButtonText: 'قبول',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.requestService
              .replyDocumentRequest({
                Id: this.request.id,
                ReplyNote: '',
                RequestStatus: this.RequestStatus.Approved,
                consigneeDepartmentId: this.form.value.consigneeDepartmentId,
              })
              .subscribe(
                () => {
                  this.loaderService.stopLoading();
                  this.alert.succuss('تمت عملية اعتماد الطلب بنجاح');
                  this.router.navigateByUrl('/requests');
                },
                (error) => {
                  this.loaderService.stopLoading();
                  console.error(error);
                  this.loaderService.stopLoading();
                  this.alert.error('فشلت عملية اعتماد الطلب !');
                }
              )
          );
        }
      });
    }
    else {
      this.dialog.open(CaseSupportingDocumentRequestReplyFormComponent, {
        width: '30em',
        data: { requestId: this.request.id, requestStatus: RequestStatus, requestType: this.request.request.requestType.id },
      });
    }
  }

  exportRequest(requestId: any) {
    let dialogRef = this.dialog.open(ExportRequestFormComponent, {
      width: '50em',
      data: { requestId: requestId, requestStatus: RequestStatus.Exported , requestType: RequestTypes.RequestSupportingDocuments}
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.router.navigateByUrl('/requests');
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
}

