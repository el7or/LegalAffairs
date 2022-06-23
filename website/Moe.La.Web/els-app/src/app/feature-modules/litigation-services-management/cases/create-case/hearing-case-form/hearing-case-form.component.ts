import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { DatePipe, Location } from '@angular/common';
import {
  FormGroup,
} from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CourtQueryObject, QueryObject } from 'app/core/models/query-objects';
import { CourtService } from 'app/core/services/court.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { GroupNames } from 'app/core/models/attachment';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { HearingType } from 'app/core/enums/HearingType';
import { CaseDetails } from 'app/core/models/case';
import { HearingDetails, SaveHearing } from 'app/core/models/hearing';
import { HearingService } from 'app/core/services/hearing.service';

@Component({
  selector: 'app-hearing-case-form',
  templateUrl: './hearing-case-form.component.html',
  styleUrls: ['./hearing-case-form.component.css']
})
export class HearingCaseFormComponent implements OnInit, OnDestroy {
  @Input('caseId') caseId: number = 0;

  @Input() case: CaseDetails;
  @Input() form: FormGroup = Object.create(null);
  @Output() onAddHearing = new EventEmitter<number>();

  hearingId: number = 0;
  saveHearing: SaveHearing = new SaveHearing();
  hearingStatus: any = [];
  hearingType: any;
  courts: any;
  _case: CaseDetails;
  hearing: HearingDetails;
  filesCount: number = 0;
  attachments: any = [];
  isPleading: boolean = false;
  queryObject: QueryObject = new QueryObject();

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
    private courtService: CourtService,
    private hijriConverter: HijriConverterService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private datePipe: DatePipe,
    public location: Location,
  ) {
  }

  ngOnInit() {
    // populate lists
    this.populateHearingStatus();
    this.populateHearingType();

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

  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      if (this.case) {
        this.form.controls['caseId'].setValue(this.case.id);
        this.onCaseChanged(this.case);

        if (this.case?.hearings?.length > 0) {
          this.patchForm(this.case?.hearings[0]);

        }
      }
    }
    if (changes['caseId']) {
      this.caseId = changes['caseId'].currentValue;
      if (this.caseId) {
        this.form.controls['caseId'].setValue(this.caseId);

      }
    }
  }

  private patchForm(hearing: HearingDetails): void {
    this.hearing = hearing;
    this.hearingId = hearing.id;

    var time = null;
    if (hearing.hearingTime) {
      const hearingTime = hearing.hearingTime.split(':');
      time = new Date();
      time.setHours(Number(hearingTime[0]));
      time.setMinutes(Number(hearingTime[1]));
    }
    this.form.patchValue({
      id: hearing.id,
      caseId: this.case.id,
      courtId: hearing.court.id,
      circleNumber: hearing.circleNumber,
      hearingNumber: hearing.hearingNumber,
      status: hearing.status.id,
      type: hearing.type.id,
      motions: hearing.motions.replace(/<br>/g, '\n'),
      hearingDate: hearing.hearingDate,
      hearingTime: time,
      closingReport: hearing.closingReport,
    });
  }

  populateHearingStatus() {
    this.subs.add(
      this.hearingService.getHearingStatus().subscribe(
        (result: any) => {
          this.loaderService.stopLoading();
          this.hearingStatus = result;

          let statusAr = result.find(x => x.value == HearingStatus.Scheduled).nameAr;
          this.form.controls['statusAr'].setValue(statusAr);
          this.updateHearingStatus();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateHearingType() {
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

  onCaseChanged(caseDetails: CaseDetails) {
    // new hearing
    if (!this.hearingId)
      this.form.controls['hearingNumber'].setValue(1);


    this._case = caseDetails;

    this.caseSubject = this._case?.subject;
    this.populateCourts(this._case?.caseSource, this._case?.litigationType);

    this.form.patchValue({
      caseId: caseDetails.id,
      litigationTypeId: this._case?.litigationType?.id,
      courtId: this._case?.court?.id,
      circleNumber: this._case?.circleNumber,
      branchId: this._case?.branchId,
      type: this.isCaseHasHearingTypeJudgment && (!this.hearingId || (this.hearingId && this.hearing?.type?.id != HearingType.PronouncingJudgment)) ? HearingType.Pleading : (this.hearingId ? this.hearing?.type?.id : null)
    });

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

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['status'].enable();
    this.form.controls['type'].enable();
    Object.assign(this.saveHearing, this.form.value);

    this.saveHearing.hearingTime = this.getTimeFormate(this.saveHearing.hearingTime);
    this.saveHearing.hearingDate = this.hijriConverter.calendarDateToDate(this.form.get('hearingDate')?.value?.calendarStart);
    this.saveHearing.attachments = this.attachments;

    var result$ = (this.form.controls["id"].value) ? this.hearingService.update(this.saveHearing) : this.hearingService.create(this.saveHearing);
    this.subs.add(
      result$.subscribe((result: any) => {
        this.loaderService.stopLoading();
        let message = this.form.controls["id"].value
          ? 'تمت عملية تعديل الجلسة بنجاح'
          : 'تمت عملية إضافة الجلسة بنجاح';
        this.alert.succuss(message);

        this.form.controls["id"].setValue(result.data.id);
        this.onAddHearing.emit(this.caseId);
      },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.form.controls['status'].disable();
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
      let statusAr = this.hearingStatus?.find(x => x.value == HearingStatus.Finished)?.nameAr;
      this.form.controls['statusAr'].setValue(statusAr);
      this.form.controls['status'].setValue(HearingStatus.Finished);
    }
    else {
      let statusAr = this.hearingStatus?.find(x => x.value == HearingStatus.Scheduled)?.nameAr;
      this.form.controls['statusAr'].setValue(statusAr);
      this.form.controls['status'].setValue(HearingStatus.Scheduled);
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
