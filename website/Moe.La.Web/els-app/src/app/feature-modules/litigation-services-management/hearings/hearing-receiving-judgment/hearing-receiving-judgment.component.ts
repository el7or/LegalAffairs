import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import * as moment from 'moment';

import { AlertService } from 'app/core/services/alert.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { QueryObject } from 'app/core/models/query-objects';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { HearingDetails } from 'app/core/models/hearing';
import { HearingService } from 'app/core/services/hearing.service';

@Component({
  selector: 'app-hearing-receiving-judgment',
  templateUrl: './hearing-receiving-judgment.component.html',
  styleUrls: ['./hearing-receiving-judgment.component.css']
})
export class HearingReceivingJudgmentComponent implements OnInit, OnDestroy {

  hearingId: number = 0;

  hearing = {} as HearingDetails;

  caseSubject = "";

  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 20,
  };

  form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(
    private activatedRouter: ActivatedRoute,
    private hearingService: HearingService,
    private hijriConverter: HijriConverterService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private router: Router,
    public location: Location) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.hearingId = +id;
      }

    });

  }

  ngOnInit() {
    this.populateHearing();
    this.form = this.fb.group({
      hearingId: [0],
      caseId: [null],
      isPronouncedJudgment: [null],
      hearingDate: [null],
      pronouncingJudgmentDate: [this.hearing.hearingDate],
      receivingJudgmentDate: [null, Validators.compose([Validators.required])],
    });
  }

  populateHearing() {
    if (this.hearingId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.hearingService.get(this.hearingId).subscribe((result: any) => {
          this.loaderService.stopLoading();
          this.hearing = result.data;
          this.caseSubject = this.hearing.case.subject;
          this.form.patchValue({
            hearingId: this.hearing.id,
            caseId: this.hearing.case.id,
            isPronouncedJudgment: this.hearing.isPronouncedJudgment,
            hearingDate: this.hearing.hearingDate,
            pronouncingJudgmentDate:this.hearing.case.pronouncingJudgmentDate?this.hearing.case.pronouncingJudgmentDate:this.hearing.hearingDate,
            receivingJudgmentDate: this.hearing.case.receivingJudgmentDate?this.hearing.case.receivingJudgmentDate:this.hearing.hearingDate,
          });
        })
      );
    }
  }
  onSubmit() {

    this.loaderService.startLoading(LoaderComponent);

    if (new Date (this.form.get('receivingJudgmentDate')?.value) == this.hearing.case.receivingJudgmentDate) {
      this.loaderService.stopLoading();
      //this.alert.error('لا يوجد اى تعديل فى البيانات')
      return;
    }
    if(this.hearing.case.receivingJudgmentDate){
      this.form.patchValue({
        isPronouncedJudgment: true,
        pronouncingJudgmentDate: this.hijriConverter.calendarDateToDate(
          this.form.get('pronouncingJudgmentDate')?.value?.calendarStart
        ),
        receivingJudgmentDate: this.hijriConverter.calendarDateToDate(
          this.form.get('receivingJudgmentDate')?.value?.calendarStart
        )
      });
    }
    else
    {
      this.form.patchValue({
        isPronouncedJudgment: true,
        receivingJudgmentDate: this.hijriConverter.calendarDateToDate(
          this.form.get('receivingJudgmentDate')?.value?.calendarStart
        )
      });
    }
    // if ((this.daysDiffPipe(this.form.get('pronouncingJudgmentDate')?.value)) > 0) {
    //   this.loaderService.stopLoading();
    //   this.alert.error('تاريخ نطق الحكم يجب ان يكون اكبر من أو يسأوي تاريخ الجلسة.');
    //   return;
    // }

    // if ((this.form.get('receivingJudgmentDate')?.value) < this.form.get('hearingDate')?.value) {
    //   this.loaderService.stopLoading();
    //   this.alert.error('تاريخ استلام الحكم يجب ان يكون اكبر من أو يسأوي تاريخ الجلسة.');
    //   return;
    // }
    var result$ = this.hearingService.receivingJudgment(this.form.value);
    this.subs.add(
      result$.subscribe(() => {
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

  daysDiffPipe(firstDate: Date): number {

    var m = moment(this.hearing.hearingDate);
    return m.diff(firstDate, 'days');

  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
