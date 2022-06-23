import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ExportCaseJudgmentRequestReplyFormComponent } from '../eport-case-judgment-request-reply-form/export-case-judgment-request-reply-form.component';
import { CaseDetails } from 'app/core/models/case';
import { CaseService } from 'app/core/services/case.service';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { Department } from 'app/core/enums/Department';
import { ExportRequestFormComponent } from '../export-request-form/export-request-form.component';
import { RequestTypes } from 'app/core/enums/RequestTypes';

@Component({
  selector: 'app-export-case-judgment-request-details',
  templateUrl: './export-case-judgment-request-details.component.html',
  styleUrls: ['./export-case-judgment-request-details.component.css']
})
export class ExportCaseJudgmentRequestDetailsComponent implements OnInit {

  caseRuleOpenState = false;
  private subs = new Subscription();
  @Input('request') request: any | undefined;
  @Input('readOnly') readOnly: boolean = false;

  public Editor = CustomEditor;
  public config = {
    language: 'ar',
    toolbar: "none"
  };

  caseDetails: CaseDetails = new CaseDetails();

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  isLitigationManager: boolean = this.authService.checkRole(
    AppRole.DepartmentManager, Department.Litigation
  );

  constructor(
    private requestService: RequestService,
    private caseService: CaseService,
    private alert: AlertService,
    private loaderService: LoaderService,
    public authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getCaseDetails(this.request.caseId);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

  getCaseDetails(caseId: any) {

    this.subs.add(
      this.caseService.get(caseId).subscribe((result: any) => {
        this.caseDetails = result.data;
      })
    );

  }

  replyExportCaseJudgmentRequest(requestStatus: RequestStatus) {
    if (requestStatus == RequestStatus.Returned) {
      let dialogRef = this.dialog.open(ExportCaseJudgmentRequestReplyFormComponent, {
        width: '30em',
        data: { requestId: this.request.id, requestStatus: RequestStatus.Returned }
      });
    }
    //else if (requestStatus == RequestStatus.Approved) {
    // let dialogRef = this.dialog.open(CaseClosingApproveFormComponent, {
    //   width: '50em',
    //   data: { requestId: this.request.id, requestStatus: RequestStatus.Approved }
    // });
    //}
    else {
      let message = '';
      let successMessage = '';
      let errorMessage = '';
      let confirmButtonText = "تقديم";

      if (requestStatus == RequestStatus.New) {
        message = 'هل أنت متأكد من إتمام عملية تقديم الطلب؟';
        successMessage = 'تمت عملية تقديم الطلب بنجاح';
        errorMessage = 'فشلت عملية تقديم الطلب !';
        confirmButtonText = "تقديم";
      }
      else if (requestStatus == RequestStatus.Approved) {
        message = 'هل أنت متأكد من إتمام عملية اعتماد الطلب ؟';
        successMessage = 'تمت عملية اعتماد الطلب  بنجاح';
        errorMessage = 'فشلت عمليةاعتماد الطلب !';
        confirmButtonText = "اعتماد";
      }
      else if (requestStatus == RequestStatus.Exported) {
        message = 'هل أنت متأكد من إتمام عملية  إغلاق القضية؟';
        successMessage = 'تمت عملية  إغلاق القضية بنجاح';
        errorMessage = 'فشلت عملية  إغلاق القضية !';
        confirmButtonText = "إغلاق";
      }
      Swal.fire({
        title: 'تأكيد',
        text: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#ff3d71',
        confirmButtonText: confirmButtonText,
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.requestService.replyExportCaseJudgmentRequest({ Id: this.request.id, ReplyNote: '', RequestStatus: requestStatus }).subscribe(
              () => {
                this.loaderService.stopLoading();
                this.alert.succuss(successMessage);
                this.router.navigateByUrl('/requests');
              },
              (error) => {
                this.loaderService.stopLoading();
                console.error(error);
                this.loaderService.stopLoading();
                this.alert.error(errorMessage);
              }
            )
          );
        }
      });
    }

  }

  exportRequest() {
    let dialogRef = this.dialog.open(ExportRequestFormComponent, {
      width: '50em',
      data: { requestId: this.request.id, requestStatus: RequestStatus.Exported , requestType: RequestTypes.RequestExportCaseJudgment }
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
