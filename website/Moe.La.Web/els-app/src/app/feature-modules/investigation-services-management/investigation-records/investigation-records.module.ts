import { NgModule } from '@angular/core';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';

import { SharedModule } from 'app/shared/shared.module';
import { InvestigationRecordListComponent } from './investigation-record-list/investigation-record-list.component';
import { InvestigationRecordFormComponent } from './investigation-record-form/investigation-record-form.component';
import { InvestigationRecordsRoutingModule } from './investigation-records-routing.module';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { DatePipe } from '@angular/common';
import { DaysDiffPipe } from 'app/shared/pipes/days-diff.pipe';
import { AttendantsListComponent } from './attendants-list/attendants-list.component';
import { AttendantFormComponent } from './attendants-form/attendants-form.component';
import { PartiesFormComponent } from './parties-form/parties-form.component';
import { PartiesListComponent } from './parties-list/parties-list.component';
import { InvestigationQuestionFormComponent } from './investigation-question-form/investigation-question-form.component';
import { PartiesDetailsComponent } from './parties-details/parties-details.component';

@NgModule({
  declarations: [InvestigationRecordListComponent, InvestigationRecordFormComponent, AttendantsListComponent, AttendantFormComponent, PartiesFormComponent, PartiesListComponent, InvestigationQuestionFormComponent, PartiesDetailsComponent],
  imports: [
    InvestigationRecordsRoutingModule,
    SharedModule,
    DatePickerDualModule,
    TimepickerModule.forRoot(),

  ],
  providers: [DatePipe, DaysDiffPipe],

})
export class InvestigationRecordsModule { }
