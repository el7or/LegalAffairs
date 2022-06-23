import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  Directive,
  Inject,
  Input,
  OnChanges,
  OnDestroy,
} from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { JDNConvertibleCalendar } from 'jdnconvertiblecalendar';
import { BehaviorSubject } from 'rxjs';

import { ACTIVE_CALENDAR } from './jdnconvertible-calendar-date-adapter/src/lib/active_calendar_token';
import { JDNConvertibleCalendarDateAdapter } from './jdnconvertible-calendar-date-adapter/src/lib/jdnconvertible-calendar-date-adapter';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';

@Component({
  selector: 'date-picker-dual',
  templateUrl: './date-picker-dual.component.html',
  styleUrls: ['./date-picker-dual.component.css'],
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    },
  ],
})
export class DatePickerDualComponent implements AfterViewChecked {
  @Input('placeholder') placeholder: string = '';
  @Input() controlName: string = '';
  @Input() required: boolean = false;


  hijriDate: any;

  constructor(
    public parentFormGroup: FormGroupDirective,
    private hijriConverter: HijriConverterService,
    private cdr: ChangeDetectorRef
  ) { }

  ngAfterViewChecked() {
    const controlValue = this.parentFormGroup.form.controls[this.controlName]
      .value;
    if (controlValue && !isNaN(new Date(controlValue).getTime())) {
      this.parentFormGroup.form.patchValue({
        [this
          .controlName]: this.hijriConverter.gregorianToGregorianCalendarDate(
            new Date(controlValue)
          ),
      });
      this.hijriDate = this.hijriConverter.gregorianToHijriCalendarDate(
        new Date(controlValue)
      );
    }
    if (!controlValue) {
      this.hijriDate = null;
    }
    this.cdr.detectChanges();
  }

  onSelecteHijri(event: any) {
    this.parentFormGroup.form.patchValue({
      [this
        .controlName]: this.hijriConverter.hijriCalendarDateToGregorianCalendarDate(
          event.value.calendarStart
        ),
    });
    this.hijriDate = event.value;
  }

  onSelectGregorian(event: any) {
    this.hijriDate = this.hijriConverter.gregorianCalendarDateToHijriCalendarDate(
      event.value.calendarStart
    );
  }
}

export const makeCalToken = () => {
  return new BehaviorSubject('Gregorian');
};

@Directive({
  selector: 'jdn-datepicker',
  providers: [
    { provide: ACTIVE_CALENDAR, useFactory: makeCalToken },
    {
      provide: DateAdapter,
      useClass: JDNConvertibleCalendarDateAdapter,
      deps: [MAT_DATE_LOCALE, ACTIVE_CALENDAR],
    },
  ],
})
export class JdnDatepicker implements OnChanges, OnDestroy {
  @Input() activeCalendar!: 'Gregorian' | 'Islamic';

  constructor(
    private adapter: DateAdapter<JDNConvertibleCalendar>,
    @Inject(ACTIVE_CALENDAR)
    private activeCalendarToken: BehaviorSubject<'Gregorian' | 'Islamic'>
  ) {
    this.adapter.setLocale('ar');
  }

  ngOnChanges(): void {
    this.activeCalendarToken.next(this.activeCalendar);
  }

  ngOnDestroy(): void {
    this.activeCalendarToken.complete();
  }
}
