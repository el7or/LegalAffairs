import { Component, OnInit, OnDestroy, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DatePipe, Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CourtService } from 'app/core/services/court.service';
import { QueryObject } from 'app/core/models/query-objects';
import { HearingDetails, SaveHearing } from 'app/core/models/hearing';
import { HearingStatus } from 'app/core/enums/HearingStatus';
import { HearingType } from 'app/core/enums/HearingType';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { Attachment, GroupNames } from 'app/core/models/attachment';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { HearingService } from 'app/core/services/hearing.service';

@Component({
  selector: 'app-hearing-summary',
  templateUrl: './hearing-summary.component.html',
  styleUrls: ['./hearing-summary.component.css'],
})
export class HearingSummaryComponent implements OnInit, AfterViewChecked, OnDestroy {
  hearingId: number = 0;

  hearingStatus: any;
  hearingType: any;
  courts: any;
  hasReceivingJudgmentDate: boolean;
  hearing = {} as HearingDetails;
  attachments: Attachment[] = [];
  filesCount: number = 0;
  caseSubject = '';
  isPleading: boolean;
  hearingSummary: SaveHearing = new SaveHearing();
  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 20,
  };
  withNewHearing: boolean = false;
  form: FormGroup = Object.create(null);
  newHearingForm: FormGroup = Object.create(null);

  private subs = new Subscription();
  hasRequiredAttachment: boolean = true;
  get HearingStatus(): typeof HearingStatus {
    return HearingStatus;
  }
  get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }
  get HearingType(): typeof HearingType {
    return HearingType;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  constructor(
    private activatedRouter: ActivatedRoute,
    private hearingService: HearingService,
    private courtService: CourtService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private datePipe: DatePipe,
    private hijriConverter: HijriConverterService,
    public location: Location,
    private cdr: ChangeDetectorRef
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.hearingId = +id;
      }
    });
  }

  ngOnInit() {
    this.initForms();
    if (this.hearingId) {
      this.loaderService.startLoading(LoaderComponent);
      this.populateCourts();
      this.populateHearingStatus();
      this.populateHearingType();
      this.subs.add(
        this.hearingService.get(this.hearingId).subscribe((result: any) => {
          this.hearing = result.data;
          this.withNewHearing =
            this.hearing.type.id == HearingType.Pleading && !this.hearing.isHasNextHearing;
          this.isPleading = this.hearing?.type?.id == HearingType.Pleading;
          this.hasReceivingJudgmentDate = this.hearing?.case?.receivingJudgmentDate ? true : false;
          this.onHearingAttachment(this.hearing.attachments);
          this.caseSubject = this.hearing.case.subject;
          if (this.withNewHearing) {
            this.validateNewHearingForm();
            this.subs.add(this.newHearingForm.controls['hearingDate'].valueChanges.subscribe(
              () => {
                this.updateHearingStatus();
              }
            ));

            this.subs.add(this.newHearingForm.controls['hearingTime'].valueChanges.subscribe(
              () => {
                this.updateHearingStatus();
              }
            ));
          }
          this.patchForms();
          //this.onCheckCloseButton();
          //this.validateNewHearingForm();
        })
      );

    }

  }
  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initForms() {
    this.form = this.fb.group({
      id: [0],
      caseId: [null, Validators.compose([Validators.required])],
      status: [1],
      statusAr: [{ value: null, disabled: true }],
      type: [""],
      courtId: [null, Validators.compose([Validators.required])],
      circleNumber: [null, Validators.compose([Validators.required])],
      attendees: [null],
      hearingNumber: [1],
      hearingDate: [null, Validators.compose([Validators.required])],
      hearingTime: [null],
      motions: [null],
      summary: [null, Validators.compose([Validators.required])],
      attachments: [null, Validators.compose([Validators.required])],
      sessionMinutes: [null],
    });
  }

  patchForms() {
    this.form.patchValue({
      id: this.hearing.id,
      caseId: this.hearing?.case?.id,
      status: this.hearing?.status?.id,
      type: this.hearing?.type?.id,
      courtId: this.hearing?.court?.id,
      circleNumber: this.hearing.circleNumber,
      attendees: this.hearing.attendees,
      hearingNumber: this.hearing.hearingNumber,
      hearingDate: this.hearing.hearingDate,
      hearingTime: this.hearing.hearingTime,
      motions: this.hearing.motions,
      summary: this.hearing.summary,
      attachments: this.attachments,
      sessionMinutes: this.hearing.sessionMinutes,
    });
    //this.onSelectHearingStatus();
    this.loaderService.stopLoading();
  }

  populateHearingStatus() {
    if (!this.hearingStatus) {
      this.subs.add(
        this.hearingService.getHearingStatus().subscribe(
          (result: any) => {
            this.hearingStatus = result;
            let statusAr = this.hearingStatus.find(x => x.value == HearingStatus.Scheduled).nameAr;
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

  populateCourts() {
    if (!this.courts) {
      this.queryObject.pageSize = 1000;
      this.subs.add(
        this.courtService.getWithQuery(this.queryObject).subscribe(
          (result: any) => {
            this.courts = result.data.items;
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

  onSelectHearingType(event: MatSelectChange) {
    this.hearing.type.id = event.value;
    //this.onCheckCloseButton();
    /* this.withNewHearing =
      this.hearing.type.id != HearingType.PronouncingJudgment; */
  }

  validateNewHearingForm() {
    if (this.withNewHearing) {

      this.newHearingForm = this.fb.group({
        id: [0],
        caseId: [null],
        status: [1],
        type: [""],
        courtId: [null],
        circleNumber: [null],
        hearingNumber: [1],
        hearingDate: [null],
        hearingTime: [null],
        motions: [null],
      });
      this.newHearingForm.patchValue({
        caseId: this.hearing.case.id,
        circleNumber: this.hearing.circleNumber,
        status: HearingStatus.Scheduled,
        courtId: this.hearing.court.id,
      });
      if (this.hearing.case.id) {
        this.getMaxHearingNumber(this.hearing.case.id);
      }
      this.newHearingForm.get('type')?.setValidators(Validators.required);
      this.newHearingForm.get('type')?.updateValueAndValidity();
      this.newHearingForm
        .get('hearingDate')
        ?.setValidators(Validators.required);
      this.newHearingForm.get('hearingDate')?.updateValueAndValidity();
      this.newHearingForm
        .get('hearingTime')
        ?.setValidators(Validators.required);
      this.newHearingForm.get('hearingTime')?.updateValueAndValidity();
      this.newHearingForm.get('motions')?.setValidators(Validators.required);
      this.newHearingForm.get('motions')?.updateValueAndValidity();
    } else {
      this.newHearingForm.get('type')?.clearValidators();
      this.newHearingForm.get('hearingDate')?.clearValidators();
      this.newHearingForm.get('hearingTime')?.clearValidators();
      this.newHearingForm.get('motions')?.clearValidators();
      this.newHearingForm.get('type')?.updateValueAndValidity();
      this.newHearingForm.get('hearingDate')?.updateValueAndValidity();
      this.newHearingForm.get('hearingTime')?.updateValueAndValidity();
      this.newHearingForm.get('motions')?.updateValueAndValidity();
    }


    /// validate new hearing date

    // this.newHearingForm.controls['hearingDate'].valueChanges.subscribe(
    //   (value) => {

    //     if (
    //       this.hijriConverter.calendarDateToDate(value?.calendarStart) <
    //       this.hijriConverter.calendarDateToDate(this.form.controls['hearingDate'].value?.calendarStart)

    //     ) {
    //       this.newHearingForm.controls['hearingDate'].setErrors({
    //         precedesOldDate: true

    //       });
    //     }
    //   }
    // );
  }

  getTimeFormate(date: string): any {
    return date != null
      ? this.datePipe.transform(new Date(date), 'HH:mm')
      : null;
  }

  getDateFormate(date: string): any {
    return this.datePipe.transform(new Date(date), 'yyyy-MM-dd');
  }

  onSubmit(status: HearingStatus) {
    this.loaderService.startLoading(LoaderComponent);

    Object.assign(this.hearingSummary, this.form.value);
    this.hearingSummary.withNewHearing = this.withNewHearing;
    if (this.withNewHearing) {
      this.hearingSummary.newHearing = new SaveHearing();
      Object.assign(this.hearingSummary.newHearing, this.newHearingForm.value);
      this.hearingSummary.newHearing!.hearingTime = this.getTimeFormate(
        this.newHearingForm.value.hearingTime
      );
      this.hearingSummary.newHearing!.hearingDate = this.hijriConverter.calendarDateToDate(
        this.newHearingForm.get('hearingDate')?.value?.calendarStart
      );
    } else {
      if (this.form.get('type')?.value == HearingType.PronouncingJudgment) {
        this.hearingSummary.isPronouncedJudgment = true;
      }
    }

    this.hearingSummary.attachments = this.attachments;
    if (status !== HearingStatus.Closed)
      this.hearingSummary.status = this.form.get('status').value;
    else
      this.hearingSummary.status = status;

    this.subs.add(
      this.hearingService.update(this.hearingSummary).subscribe(
        () => {
          this.loaderService.stopLoading();

          let message = 'تمت عملية الحفظ بنجاح';
          this.alert.succuss(message);
          this.location.back();
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = 'فشلت عملية الحفظ !';
          this.alert.error(message);
        }
      )
    );
  }

  onHearingAttachment(list: any[]) {
    if (this.hearing.type.id == HearingType.PronouncingJudgment) {
      this.hasRequiredAttachment = list.filter(a => a.attachmentType == "ضبط الجلسة" || a.attachmentTypeId == 1).length > 0;
    }

    this.attachments = list;
        this.filesCount = this.attachments.filter(a => !a.isDeleted).length;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  getMaxHearingNumber(caseId: number) {
    this.subs.add(
      this.hearingService
        .getMaxHearingNumber(caseId)
        .subscribe((result: any) => {
          this.loaderService.stopLoading();
          this.newHearingForm.controls['hearingNumber'].setValue(
            result.data + 1
          );
        })
    );
  }

  private updateHearingStatus(): void {
    let dateVal = this.newHearingForm.controls['hearingDate'].value
    let timeVal = new Date(this.newHearingForm.controls['hearingTime'].value);
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
      this.newHearingForm.controls['status'].setValue(2);
    }
    else {
      this.newHearingForm.controls['status'].setValue(1);
    }
  }
}
