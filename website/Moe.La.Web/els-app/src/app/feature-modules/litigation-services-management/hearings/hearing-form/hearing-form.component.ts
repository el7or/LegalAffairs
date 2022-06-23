import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { DatePipe, Location } from '@angular/common';
import {
  FormGroup,
  FormBuilder,
  Validators
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseQueryObject, CourtQueryObject, QueryObject } from 'app/core/models/query-objects';
import { CourtService } from 'app/core/services/court.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { GroupNames } from 'app/core/models/attachment';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { HearingType } from 'app/core/enums/HearingType';
import { HearingSearchCasesComponent } from '../hearing-search-cases/hearing-search-cases.component';
import { CaseDetails, CaseListItem } from 'app/core/models/case';
import { HearingDetails, SaveHearing } from 'app/core/models/hearing';
import { CaseService } from 'app/core/services/case.service';
import { HearingService } from 'app/core/services/hearing.service';

@Component({
  selector: 'app-hearing-form',
  templateUrl: './hearing-form.component.html',
  styleUrls: ['./hearing-form.component.css'],
})
export class HearingFormComponent implements OnInit, OnDestroy {

  hearingId: number = 0;
  caseId: number = 0;
  saveHearing: SaveHearing = new SaveHearing();
  editable: boolean;
  hearingStatus: any;
  hearingType: any;
  courts: any;
  _case: CaseDetails;
  cases: CaseListItem[] = [];
  hearing: HearingDetails;
  filesCount: number = 0;
  attachments: any = [];
  isPleading: boolean = false;
  queryObject: QueryObject = new QueryObject();
  caseQueryObject: CaseQueryObject = new CaseQueryObject();
  filteredCases$: Observable<CaseListItem[]> = new Observable<CaseListItem[]>();
  hasReceivingJudgmentDate: boolean = false;
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  caseSubject: string = "";
  isCaseHasHearingTypeJudgment: boolean = false;

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  get HearingType(): typeof HearingType {
    return HearingType;
  }

  constructor(
    private hearingService: HearingService,
    private caseService: CaseService,
    private courtService: CourtService,
    private hijriConverter: HijriConverterService,
    private datePipe: DatePipe,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private activatedRouter: ActivatedRoute,
    private router: Router,
    public location: Location,
    private dialog: MatDialog
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.hearingId = +id;
      }
    });

    this.activatedRouter.queryParams.subscribe((params) => {
      this.caseId = params.case;
    });


  }

  ngOnInit() {
    this.init();

    //new hearing with case id
    if (this.caseId) {
      this.form.controls['caseId'].setValue(this.caseId);
      this.onCaseChanged(this.form.value.caseId);
    }

    // populate lists
    this.populateHearingStatus();
    this.populateHearingType();
    // this.populateCourts();
    ///

    if (this.hearingId) {
      this.populateHearing()
    }

    this.subs.add(this.form.controls['hearingDate'].valueChanges.subscribe(
      () => {
        this.updateHearingStatus();
      }
    ));

    this.subs.add(this.form.controls['hearingTime'].valueChanges.subscribe(
      () => {
        this.updateHearingStatus();
      }
    ));
  }

  populateHearing() {
    //update hearing
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.hearingService.get(this.hearingId).subscribe((result: any) => {
        this.hearing = result.data;

        this.patchForm(this.hearing);
        this.onCaseChanged(this.hearing.case.id);
        //this.loaderService.stopLoading();
      }, (error) => {
        console.error(error);
        this.alert.error('فشلت عملية جلب البيانات !');
        this.loaderService.stopLoading();
      })
    );
  }

  populateHearingStatus() {
    if (!this.hearingStatus) {
      this.subs.add(
        this.hearingService.getHearingStatus().subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.hearingStatus = result;
            let statusAr = result.find(x => x.value == HearingStatus.Scheduled).nameAr;
            this.form.controls['statusAr'].setValue(statusAr);
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
  }

  populateHearingType() {
    if (!this.hearingType) {
      this.subs.add(
        this.hearingService.getHearingType().subscribe(
          (result: any) => {
            this.hearingType = result;
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
  }

  populateCourts(category, litigationType) {
    let queryObject: CourtQueryObject = new CourtQueryObject({
      pageSize: 999,
      courtCategory: category?.id,
      litigationType: litigationType?.id
    });
    this.subs.add(
      this.courtService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.courts = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  onChooseCase() {
    this.caseQueryObject.isForHearingAddition = true;

    const dialogRef = this.dialog.open(HearingSearchCasesComponent, {
      width: '90%',
      height: '90%',
      data: this.caseQueryObject
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res: CaseListItem) => {
          if (res) {
            this.caseId = res.id;

            this.onCaseChanged(res.id);
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  onCaseChanged(caseId: any) {
    // new hearing
    if (!this.hearingId)
      this.getMaxHearingNumber(caseId);

    //if (!this.hearingId) {
    this.loaderService.startLoading(LoaderComponent);

    ///
    this.subs.add(
      this.caseService.get(caseId).subscribe((result: any) => {
        this._case = result.data;
        this.caseSubject = this._case?.subject;
        this.populateCourts(this._case?.caseSource, this._case?.litigationType);

        if (this._case?.hearings.find(c => c.type == "نطق بالحكم") && (!this.hearingId || (this.hearingId && this.hearing.type.id != HearingType.PronouncingJudgment))) {
          this.isCaseHasHearingTypeJudgment = true;
          this.form.controls['type'].disable();
        }

        this.form.patchValue({
          caseId: caseId,
          litigationTypeId: this._case?.litigationType?.id,
          courtId: this._case?.court?.id,
          circleNumber: this._case?.circleNumber,
          branchId: this._case?.branchId,
          type: this.isCaseHasHearingTypeJudgment && (!this.hearingId || (this.hearingId && this.hearing?.type?.id != HearingType.PronouncingJudgment)) ? HearingType.Pleading : (this.hearingId ? this.hearing?.type?.id : null)
        });
        this.loaderService.stopLoading();
      },
        err => this.loaderService.stopLoading())
    );
    // }
  }

  getMaxHearingNumber(caseId: number) {
    this.subs.add(
      this.hearingService
        .getMaxHearingNumber(caseId)
        .subscribe((result: any) => {
          this.form.controls['hearingNumber'].setValue(result.data + 1);
        })
    );
  }

  isHearingNumberExist() {
    const hearingNumberCtrl = this.form.get('hearingNumber');

    if (this.form.value.hearingNumber && this.form.value.caseId) {

      this.subs.add(
        this.hearingService
          .isHearingNumberExists(this.form.value)
          .subscribe((res: any) => {
            if (res.data)
              hearingNumberCtrl?.setErrors({ hearingNumberExists: true });
          })
      );
    }
  }

  onHearingAttachment(list: any) {
    this.attachments = list;
    this.filesCount = this.attachments.filter(a => !a.isDeleted).length;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['status'].enable();
    this.form.controls['type'].enable();
    Object.assign(this.saveHearing, this.form.value);

    this.saveHearing.hearingTime = this.getTimeFormate(this.saveHearing.hearingTime);
    this.saveHearing.hearingDate = this.hijriConverter.calendarDateToDate(this.form.get('hearingDate')?.value?.calendarStart);
    this.saveHearing.attachments = this.attachments;

    var result$ = (this.hearingId) ? this.hearingService.update(this.saveHearing) : this.hearingService.create(this.saveHearing);
    this.subs.add(
      result$.subscribe((result: any) => {
        this.loaderService.stopLoading();
        let message = this.hearingId
          ? 'تمت عملية التعديل بنجاح'
          : 'تمت عملية الإضافة بنجاح';
        this.alert.succuss(message);
        this.router.navigateByUrl('/hearings/view/' + result.data.id);

      },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.form.controls['status'].disable();
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );

  }

  getTimeFormate(date: string): any {
    return date != null
      ? this.datePipe.transform(new Date(date), 'HH:mm')
      : null;
  }

  getDateFormate(date: string): any {
    return this.datePipe.transform(new Date(date), 'yyyy-MM-dd');
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  // Private methods.
  private updateHearingStatus(): void {
    let dateVal = this.form.controls['hearingDate'].value;
    let timeVal = new Date(this.form.controls['hearingTime'].value);

    if (!dateVal?.calendarStart) {
      return;
    }

    let year = dateVal?.calendarStart.year;
    let month = dateVal?.calendarStart.month - 1;
    let day = dateVal?.calendarStart.day;
    let hours = timeVal?.getHours();
    let minutes = timeVal?.getMinutes();
    let milliseconds = timeVal?.getMilliseconds();

    if (dateVal?.calendarStart && (new Date(year, month, day, hours, minutes, milliseconds) < new Date())) {
      let statusAr = this.hearingStatus?.find(x => x.value == HearingStatus.Finished).nameAr;
      this.form.controls['statusAr'].setValue(statusAr);
      this.form.controls['status'].setValue(HearingStatus.Finished);
    }
    else {
      let statusAr = this.hearingStatus.find(x => x.value == HearingStatus.Scheduled).nameAr;
      this.form.controls['statusAr'].setValue(statusAr);
      this.form.controls['status'].setValue(HearingStatus.Scheduled);
    }
  }

  private init(): void {
    this.form = this.fb.group({
      id: [0],
      caseId: [null, Validators.compose([Validators.required])],
      courtId: ["", Validators.compose([Validators.required])],
      litigationTypeId: [null],
      //assignedToId: [null],
      hearingNumber: [1, Validators.compose([Validators.required])],
      status: [{ value: 1, disabled: true }],
      statusAr: [{ value: null, disabled: true }],
      type: ["", Validators.compose([Validators.required])],
      hearingDate: [null, Validators.compose([Validators.required])],
      hearingTime: [null, Validators.compose([Validators.required])],
      circleNumber: [null],
      motions: [null, Validators.compose([Validators.required])],
      branchId: [null, Validators.compose([Validators.required])],
      attachments: [null]
    });
  }

  private patchForm(hearing: HearingDetails): void {
    this.onHearingAttachment(hearing.attachments);

    var time = null;
    if (this.hearing.hearingTime) {
      const hearingTime = this.hearing.hearingTime.split(':');
      time = new Date();
      time.setHours(Number(hearingTime[0]));
      time.setMinutes(Number(hearingTime[1]));
    }
    this.form.patchValue({
      id: hearing.id,
      caseId: hearing.case.id,
      courtId: hearing.court.id,
      circleNumber: hearing.circleNumber,
      hearingNumber: hearing.hearingNumber,
      status: hearing.status.id,
      type: hearing.type.id,
      motions: hearing.motions,
      hearingDate: hearing.hearingDate,
      hearingTime: time,
      closingReport: hearing.closingReport,
    });
  }

}
