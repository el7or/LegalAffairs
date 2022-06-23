import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';

import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Department } from 'app/core/enums/Department';
import { AppRole } from 'app/core/models/role';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { ChangeHearingResearcherAcceptFormComponent } from '../change-hearing-researcher-accept-form/change-hearing-researcher-accept-form.component';
import { ChangeHearingResearcherRejectFormComponent } from '../change-hearing-researcher-reject-form/change-hearing-researcher-reject-form.component';

@Component({
  selector: 'app-change-hearing-researcher-request-details',
  templateUrl: './change-hearing-researcher-request-details.component.html',
  styleUrls: ['./change-hearing-researcher-request-details.component.css']
})
export class ChangeHearingResearcherRequestDetailsComponent implements OnInit, OnDestroy {

  @Input('request') requestFull: any | undefined;

  private subs = new Subscription();

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);

  constructor(
    private alert: AlertService,
    private matDialog: MatDialog,
    private authService: AuthService,
    public location: Location,
    private router: Router) { }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }


  onAccept() {
    const dialogRef = this.matDialog.open(ChangeHearingResearcherAcceptFormComponent, {
      width: '30em',
      data: {
        requestId: this.requestFull.request.id,
        researcherId: this.requestFull.currentResearcher.id,
        newResearcherId: this.requestFull.newResearcher.id,
      }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            this.alert.succuss('تم الموافقة على طلب تغيير الباحث بنجاح');
            this.requestFull.replyNote = result;
            this.requestFull.request.requestStatus = {
              id: this.RequestStatus.Accepted,
              name: 'مقبولة'
            };
            this.router.navigateByUrl('requests');
          }
        }
      )
    );
  }

  onReject() {
    const dialogRef = this.matDialog.open(ChangeHearingResearcherRejectFormComponent, {
      width: '30em',
      data: {
        requestId: this.requestFull.request.id,
      }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            this.alert.succuss('تم رفض تغيير الباحث بنجاح');
            this.requestFull.replyNote = result;
            this.requestFull.request.requestStatus = {
              id: this.RequestStatus.Rejected,
              name: 'مرفوضة'
            };
            this.router.navigateByUrl('requests');
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
}
