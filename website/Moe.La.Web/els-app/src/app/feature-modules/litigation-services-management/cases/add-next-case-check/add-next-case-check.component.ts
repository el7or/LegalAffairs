import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { SuggestedOpinon } from 'app/core/enums/SuggestedOpinon';
import { ObjectionPermitRequestFormComponent } from '../../requests/objection-permit-request-form/objection-permit-request-form.component';
import { AddCaseObjectionMemoComponent } from '../add-case-objection-memo/add-case-objection-memo.component';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { JudgementResult } from 'app/core/enums/JudgementResult';
import { AddNextCaseComponent } from '../add-next-case/add-next-case.component';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-next-case-check',
  templateUrl: './add-next-case-check.component.html',
  styleUrls: ['./add-next-case-check.component.css']
})
export class AddNextCaseCheckComponent implements OnInit {
  private subs = new Subscription();
  caseId: any;
  selectedIndex: StepsIndexes = StepsIndexes.PermitRequest;
  case: any;
  finishedPronouncedHearing: any;
  permitRequest: any;
  showObjectionPermitRequest: boolean = false;
  objectionRequest: any;
  litigationType: any;
  isPermitRequestAccepted: boolean;
  public get SuggestedOpinon(): typeof SuggestedOpinon {
    return SuggestedOpinon;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }

  public get StepsIndexes(): typeof StepsIndexes {
    return StepsIndexes;
  }

  constructor(
    private dialogRef: MatDialogRef<AddNextCaseCheckComponent>,
    private caseService: CaseService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private dialog: MatDialog,
    private alert: AlertService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.caseId = this.data.case?.id;
    this.case = this.data.case;
  }

  ngOnInit(): void {
    this.populateCaseDetails();
  }

  populateCaseDetails() {
    if (this.caseId) {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.caseService.get(this.caseId).subscribe(
          (res: any) => {
            this.litigationType = res.data.litigationType.id;

            // show objectionpermit request step only if judgementResult is Favor
            if (res.data?.caseRule?.judgementResult?.id == JudgementResult.Favor) {
              this.showObjectionPermitRequest = true;

              // check current objection request
              this.getCaseObjectionPermitRequest();

            }
            else {
              // if judgementResult not Favor then first step(MemoRequest)
              this.selectedIndex = StepsIndexes.MemoRequest;

              // check current objection memo request
              this.getCaseObjectionMemoRequest();

            }
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت  عملية جلب البيانات');
            this.loaderService.stopLoading();
          }
        )
      );

    }
  }

  getCaseObjectionPermitRequest() {
    this.subs.add(
      this.requestService.getCaseObjectionPermitRequest(this.caseId).subscribe(
        (res: any) => {

          this.permitRequest = res.data;

          // if current objection permit request approved from litigationmanager go to next step (MemoRequest)
          if (this.permitRequest?.request?.requestStatus == RequestStatus.AcceptedFromLitigationManager) {
            if (this.permitRequest?.suggestedOpinon == SuggestedOpinon.ObjectionAction) {
              this.selectedIndex = StepsIndexes.MemoRequest;
              // get current objectionmemorequest details
              this.getCaseObjectionMemoRequest();
            }
            else {
              this.selectedIndex = StepsIndexes.PermitRequest;
              this.loaderService.stopLoading();
            }


          }
          else
            this.loaderService.stopLoading();

        }, (error => {
          this.loaderService.stopLoading();

        })));
  }

  getCaseObjectionMemoRequest() {
    this.subs.add(
      this.requestService.getCaseObjectionMemoRequest(this.caseId).subscribe(
        (res: any) => {
          this.loaderService.stopLoading();

          this.objectionRequest = res.data;

          // if current objection request acceptes go to next step (NewCase)
          if (this.objectionRequest?.request?.requestStatus.id == RequestStatus.Accepted)
            this.selectedIndex = StepsIndexes.NewCase;


        }, (error => {
          this.loaderService.stopLoading();

        })));
  }


  openObjectionPermitRequestModal(): void {
    const newdialogRef = this.dialog.open(ObjectionPermitRequestFormComponent, {
      width: '30em',
      data: { caseId: this.caseId },
    });

    this.subs.add(
      newdialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCaseDetails();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  openSelectMemoModal() {
    const newdialogRef = this.dialog.open(AddCaseObjectionMemoComponent, {
      width: '50em',
      data: { caseId: this.caseId, secondSubCategoryId: this.case.secondSubCategoryId },
    });
    this.subs.add(
      newdialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateCaseDetails();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  addNextCase() {
    const newdialogRef = this.dialog.open(AddNextCaseComponent, {
      width: '30em',
      data: { case: this.case },
    });
    this.subs.add(
      newdialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.dialogRef.close();
            this.router.navigate(['/cases/view', res.data.id]);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }
}

enum StepsIndexes {
  PermitRequest,
  MemoRequest,
  NewCase
}
