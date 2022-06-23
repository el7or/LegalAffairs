import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HearingCaseFormComponent } from '../cases/create-case/hearing-case-form/hearing-case-form.component';
 
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { UploadMultipleFilesModule } from 'app/shared/components/upload-multiple-files/upload-multiple-files.module';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { SharedModule } from 'app/shared/shared.module';
import { DaysDiffPipe } from 'app/shared/pipes/days-diff.pipe';
import { HearingSearchCasesComponent } from '../hearings/hearing-search-cases/hearing-search-cases.component';
import { CaseDetailsComponent } from '../cases/case-details/case-details.component';
import { JudgmentReceivedComponent } from '../cases/judgment-received/judgment-received.component';
 
@NgModule({
  declarations: [
   
    HearingSearchCasesComponent,
    CaseDetailsComponent,
    JudgmentReceivedComponent
   ],
  imports: [
    SharedModule,
    DatePickerDualModule,
    UploadMultipleFilesModule,
    TimepickerModule.forRoot(),
    CKEditorModule,
  ],
  providers: [DatePipe, DaysDiffPipe],
  exports: [ HearingSearchCasesComponent, CaseDetailsComponent]
})
export class CaseHearingSharedModule { }
