import { MoamalatServicesManagementModule } from './../../moamalat-services-management/moamalat-services-management.module';
import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';

import { SharedModule } from 'app/shared/shared.module';
import { CasesRoutingModule } from './cases-routing.module';
import { CaseListComponent } from './case-list/case-list.component';
import { CaseViewComponent } from './case-view/case-view.component';
import { ChangeCaseStatusFormComponent } from './change-case-status-form/change-case-status-form.component';
import { SentToBranchManagerFormComponent } from './send-to-general-manager-form/send-to-general-manager-form.component';
import { AssignCaseToResearcherComponent } from './assign-case-to-researcher/assign-case-to-researcher.component';
import { CaseFormComponent } from './case-form/case-form.component';
import { CaseHearingsComponent } from './case-hearings/case-hearings.component';
import { CaseLegalMemosComponent } from './case-legal-memos/case-legal-memos.component';
import { JudgmentFormComponent } from './judgment-form/judgment-form.component';
import { CaseRelatedsComponent } from './case-relateds/case-relateds.component';
import { UploadMultipleFilesModule } from 'app/shared/components/upload-multiple-files/upload-multiple-files.module';
import { InitialCaseFormComponent } from './initial-case-form/initial-case-form.component';
import { ManualPartiesListComponent } from './manual-parties-list/manual-parties-list.component';
import { AddManualPartyFormComponent } from './add-manual-party-form/add-manual-party-form.component';
import { CreateCaseComponent } from './create-case/create-case.component';
import { BasicCaseFormComponent } from './create-case/basic-case-form/basic-case-form.component';
import { PartyCaseFormComponent } from './create-case/party-case-form/party-case-form.component';
import { AddPartyComponent } from './create-case/party-case-form/add-party/add-party.component';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { DaysDiffPipe } from 'app/shared/pipes/days-diff.pipe';
import { SearchPartyComponent } from './create-case/party-case-form/search-party/search-party.component';
import { CaseGroundsFormComponent } from './create-case/case-grounds/case-grounds-list/case-grounds-list.component';
import { CaseSummaryComponent } from './create-case/case-summary/case-summary.component';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { AddCaseObjectionMemoComponent } from './add-case-objection-memo/add-case-objection-memo.component';
import { AddNextCaseCheckComponent } from './add-next-case-check/add-next-case-check.component';
import { AddNextCaseComponent } from './add-next-case/add-next-case.component';
import { CaseGroundUpdateFormComponent } from './create-case/case-grounds/case-ground-form/case-ground-form.component';
import { HearingsModule } from '../hearings/hearings.module';
import { HearingCaseFormComponent } from './create-case/hearing-case-form/hearing-case-form.component';
import { CaseHearingSharedModule } from '../caseHearingShared/cases-hearing-shared.module';
import { CaseMoamalaFormComponent } from './create-case/case-moamala-form/case-moamala-form.component';
import { SearchMoamalaComponent } from './create-case/case-moamala-form/search-moamala/search-moamala.component';
import { CasemoamalatComponent } from './case-moamalat/case-moamalat.component';

@NgModule({
  declarations: [
    CaseListComponent,
    CaseViewComponent,
    ChangeCaseStatusFormComponent,
    SentToBranchManagerFormComponent,
    AssignCaseToResearcherComponent,
    CaseFormComponent,
    CaseHearingsComponent,
    CaseLegalMemosComponent,
    JudgmentFormComponent,
    CaseRelatedsComponent,
    InitialCaseFormComponent,
    ManualPartiesListComponent,
    AddManualPartyFormComponent,
    CreateCaseComponent,
    BasicCaseFormComponent,
    PartyCaseFormComponent,
    AddPartyComponent,
    SearchPartyComponent,
    CaseGroundsFormComponent,
    CaseSummaryComponent,
    AddCaseObjectionMemoComponent,
    AddNextCaseCheckComponent,
    AddNextCaseComponent,
    CaseGroundUpdateFormComponent,
    HearingCaseFormComponent,
    CaseMoamalaFormComponent,
    SearchMoamalaComponent,
    CasemoamalatComponent
  ],
  imports: [
    SharedModule,
    CasesRoutingModule,
    DatePickerDualModule,
    UploadMultipleFilesModule,
    TimepickerModule.forRoot(),
    CKEditorModule,
    HearingsModule,
    MoamalatServicesManagementModule,
    CaseHearingSharedModule
  ],
  providers: [DatePipe, DaysDiffPipe],
  exports: []
})
export class CasesModule { }
