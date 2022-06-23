import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import {
  DatePickerDualComponent,
  JdnDatepicker,
  makeCalToken,
} from "./date-picker-dual.component";
import { ACTIVE_CALENDAR, JDNConvertibleCalendarDateAdapter, MatJDNConvertibleCalendarDateAdapterModule } from "../date-picker-dual/jdnconvertible-calendar-date-adapter/src/public_api";
import { CommonModule } from '@angular/common';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';

@NgModule({
  declarations: [DatePickerDualComponent, JdnDatepicker],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatJDNConvertibleCalendarDateAdapterModule,
  ],
  exports: [DatePickerDualComponent],
  providers:[
    { provide: ACTIVE_CALENDAR, useFactory: makeCalToken },
    {
      provide: DateAdapter,
      useClass: JDNConvertibleCalendarDateAdapter,
      deps: [MAT_DATE_LOCALE, ACTIVE_CALENDAR],
    },
  ]
})
export class DatePickerDualModule {}
