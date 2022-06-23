import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';

import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Department } from 'app/core/enums/Department';
import { AppRole } from 'app/core/models/role';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { ChangeResearcherAcceptFormComponent } from '../change-researcher-accept-form/change-researcher-accept-form.component';
import { ChangeResearcherRejectFormComponent } from '../change-researcher-reject-form/change-researcher-reject-form.component';

@Component({
  selector: 'app-change-researcher-request-details',
  templateUrl: './change-researcher-request-details.component.html',
  styleUrls: ['./change-researcher-request-details.component.css']
})
export class ChangeResearcherRequestDetailsComponent implements OnInit {

  @Input('request') requestFull: any | undefined;

  private subs = new Subscription();

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  isRegionsSupervisor: boolean = this.authService.checkRole(AppRole.RegionsSupervisor);
  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);
  isBranchManager: boolean = this.authService.checkRole(AppRole.BranchManager);

  constructor(
    private alert: AlertService,
    private matDialog: MatDialog,
    private authService: AuthService,
    public location: Location,
    private router: Router) { }

  ngOnInit(): void {
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }


  onAccept() {
    const dialogRef = this.matDialog.open(ChangeResearcherAcceptFormComponent, {
      width: '30em',
      data: {
        requestId: this.requestFull.request.id,
        raesearcherId: this.requestFull.currentResearcher.id
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
    const dialogRef = this.matDialog.open(ChangeResearcherRejectFormComponent, {
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
