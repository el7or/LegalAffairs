import { Component, OnInit, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { ObjectionPermitRequestReplyFormComponent } from '../objection-permit-request-reply-form/objection-permit-request-reply-form.component';
import { Department } from 'app/core/enums/Department';
import { ObjectionPermitRequestDetails } from 'app/core/models/request';
import { SuggestedOpinon } from 'app/core/enums/SuggestedOpinon';

@Component({
  selector: 'app-objection-permit-request-details',
  templateUrl: './objection-permit-request-details.component.html',
  styleUrls: ['./objection-permit-request-details.component.css']
})
export class ObjectionPermitRequestDetailsComponent implements OnInit {

  @Input('request') request: ObjectionPermitRequestDetails;

  public get Department(): typeof Department {
    return Department;
  }
  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  private subs = new Subscription();

  constructor(
    public authService: AuthService,
    private dialog: MatDialog,
    public location: Location
  ) { }

  ngOnInit(): void {
  }
  ngDestroy() {
    this.subs.unsubscribe();
  }

  ReplyObjectionRequest(requestStatus: RequestStatus) {

    // let requestStatus:RequestStatus;

    // if(requestAccepted)
    //    if(this.request.suggestedOpinon.id==SuggestedOpinon.ObjectionAction)
    //      requestStatus=RequestStatus.AcceptedFromLitigationManager;
    //    else
    //      requestStatus=RequestStatus.Rejected
    // else //rejected
    //    if(this.request.suggestedOpinon.id==SuggestedOpinon.ObjectionAction)
    //       requestStatus=RequestStatus.Rejected;
    //    else
    //       requestStatus=RequestStatus.AcceptedFromLitigationManager

    this.dialog.open(ObjectionPermitRequestReplyFormComponent, {
      width: '30em',
      data: { requestId: this.request.id, caseId: this.request.caseId, requestStatus: requestStatus, suggestedOpinon:this.request.suggestedOpinon}
    });
  }
}


