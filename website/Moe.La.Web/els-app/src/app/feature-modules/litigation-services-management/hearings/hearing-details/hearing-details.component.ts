import { Component, OnInit, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { HearingLegalMemoFormComponent } from '../hearing-legal-memo-form/hearing-legal-memo-form.component';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { HearingDetails } from 'app/core/models/hearing';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HearingType } from 'app/core/enums/HearingType';
import { HearingUpdateQueryObject } from 'app/core/models/query-objects';
import { Department } from 'app/core/enums/Department';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { AlertService } from 'app/core/services/alert.service';
import { GroupNames } from 'app/core/models/attachment';
import { HearingService } from 'app/core/services/hearing.service';
import { HearingAssignToFormComponent } from '../hearing-assign-to-form/hearing-assign-to-form.component';

@Component({
  selector: 'app-hearing-details',
  templateUrl: './hearing-details.component.html',
  styleUrls: ['./hearing-details.component.css'],
})
export class HearingDetailsComponent implements OnDestroy {

  hasApprovedAddedLegalMemo?: boolean;
  hasAddingLegalMemoRequest?: boolean;
  requestsDisplayedColumns = ['position', 'requestType', 'requestStatus', 'createdByFullName', 'createdOn', 'actions'];
  isRelatedMemosExist: boolean = false;
  hearingId: number = 0;
  private subs = new Subscription();
  hearing: HearingDetails = new HearingDetails();
  isAssignedTo: boolean = false;
  isLastHearing: boolean = false;
  currentDateTime: Date = new Date();
  hearingDateTime!: Date;
  hearingUpdatestotalItems: number = 0;
  isClosedCase: boolean = false;
  hasReceivingJudgmentDate: boolean = false;
  doneJudgment: boolean = false;
  editable: boolean = true;
  todayDate?: Date;
  receivingJudgmentDate?: Date;

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public get RequestTypes(): typeof RequestTypes {
    return RequestTypes;
  }
  get HearingStatus(): typeof HearingStatus {
    return HearingStatus;
  }
  get HearingType(): typeof HearingType {
    return HearingType;
  }
  get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);

  constructor(
    private activatedRouter: ActivatedRoute,
    private dialog: MatDialog,
    public authService: AuthService,
    private hearingService: HearingService,
    private loaderService: LoaderService,
    public location: Location,
    private legalMemoService: LegalMemoService,
    private alert: AlertService,

  ) {
    this.activatedRouter.params.subscribe(params => {
      this.hearingId = +params['id'];
      this.getHearingUpdates();
      this.populateHearing();
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateHearing() {
    if (this.hearingId) {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.hearingService.get(this.hearingId).subscribe((result: any) => {
          this.loaderService.stopLoading();
          this.hearing = result.data;

          this.hearingDateTime = new Date(this.hearing.hearingDate);

          if (this.hearing.case.closeDate != null) {
            this.isClosedCase = true;
          }
          if (this.hearing.case.receivingJudgmentDate != null) {
            this.hasReceivingJudgmentDate = true;
            this.receivingJudgmentDate = new Date(this.hearing.case.receivingJudgmentDate);
            this.receivingJudgmentDate = new Date(this.receivingJudgmentDate.getFullYear(), this.receivingJudgmentDate.getMonth(), this.receivingJudgmentDate.getDate());
            const currenDate = new Date();
            this.todayDate = new Date(currenDate.getFullYear(), currenDate.getMonth(), currenDate.getDate());
          }
          if (this.hearing?.case?.caseRule?.isFinalJudgment) {
            this.doneJudgment = true;
          }
          this.editable = ((!this.hasReceivingJudgmentDate || this.hearing.isPronouncedJudgment) && !this.hearing?.case?.caseRule?.isFinalJudgment && !this.isClosedCase);

          if (this.hearing.assignedTo) {
            this.isAssignedTo =
              this.authService.currentUser?.id == this.hearing.assignedTo?.id;
          }
          if (this.hearing.hearingNumber && this.hearing.case.id) {
            this.getMaxHearingNumber(this.hearing.case.id);
          }
        })
      );
    }
  }
  openDialog() {
    const dialogRef = this.dialog.open(HearingLegalMemoFormComponent, {
      width: '50em',
      data: {
        id: this.hearingId,
        secondSubCategoryId: this.hearing.case.secondSubCategory?.id
      },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateHearing();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  getMaxHearingNumber(caseId: number) {
    this.subs.add(
      this.hearingService
        .getMaxHearingNumber(caseId)
        .subscribe((result: any) => {
          this.loaderService.stopLoading();
          //check there is no hearing after this hearing
          this.isLastHearing = result.data == this.hearing.hearingNumber;
        })
    );
  }

  getHearingUpdates() {
    var queryObject: HearingUpdateQueryObject = new HearingUpdateQueryObject({
      sortBy: 'text',
      pageSize: 999,
      hearingId: this.hearingId
    });
    this.subs.add(
      this.hearingService.getAllHearingUpdates(queryObject).subscribe(
        (result: any) => {
          this.hearingUpdatestotalItems = result.data.totalItems;
        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }
  onPrint(memoId) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.printHearingLegalMemo({ hearingId: this.hearing.id, legalMemoId: memoId }).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.target = '_blank';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل طباعة البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onAssign(hearing: HearingDetails) {
    const dialogRef = this.dialog.open(HearingAssignToFormComponent, {
      width: '30em',
      data: {
        hearingId: hearing.id,
        attendantId: hearing.assignedTo?.id,
      },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            hearing.assignedTo = res;
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

}
