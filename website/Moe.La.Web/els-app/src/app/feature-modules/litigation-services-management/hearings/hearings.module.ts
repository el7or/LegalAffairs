import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HearingSummaryViewComponent } from './hearing-summary-view/hearing-summary-view.component';
import { ReceiveJudgmentInstrumentComponent } from './receive-judgment-instrument/receive-judgment-instrument.component';
import { HearingUpdateFormComponent } from './hearing-update-form/hearing-update-form.component';
import { HearingUpdatesListComponent } from './hearing-updates-list/hearing-updates-list.component';
import { HearingAssignToFormComponent } from './hearing-assign-to-form/hearing-assign-to-form.component';
import { HearingLegalMemoFormComponent } from './hearing-legal-memo-form/hearing-legal-memo-form.component';
import { HearingReceivingJudgmentComponent } from './hearing-receiving-judgment/hearing-receiving-judgment.component';
import { HearingSummaryComponent } from './hearing-summary/hearing-summary.component';
import { HearingDetailsComponent } from './hearing-details/hearing-details.component';
import { HearingFormComponent } from './hearing-form/hearing-form.component';
import { HearingListComponent } from './hearing-list/hearing-list.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { UploadMultipleFilesModule } from 'app/shared/components/upload-multiple-files/upload-multiple-files.module';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { HearingsRoutingModule } from './hearings-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { DaysDiffPipe } from 'app/shared/pipes/days-diff.pipe';
import { CaseHearingSharedModule } from '../caseHearingShared/cases-hearing-shared.module';
 
@NgModule({
  declarations: [
    HearingListComponent,
    HearingFormComponent,
    HearingDetailsComponent,
    HearingSummaryComponent,
    HearingReceivingJudgmentComponent,
    HearingLegalMemoFormComponent,
    HearingAssignToFormComponent,
    HearingUpdatesListComponent,
    HearingUpdateFormComponent,
    ReceiveJudgmentInstrumentComponent,
    HearingSummaryViewComponent
   ],
  imports: [
    SharedModule,
    HearingsRoutingModule,
    DatePickerDualModule,
    UploadMultipleFilesModule,
    TimepickerModule.forRoot(),
    CKEditorModule,
    CaseHearingSharedModule
  ],
  providers: [DatePipe, DaysDiffPipe],
  exports: [ HearingFormComponent]
})
export class HearingsModule { }
