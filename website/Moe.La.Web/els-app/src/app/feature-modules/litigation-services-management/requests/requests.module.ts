import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';

import { SharedModule } from 'app/shared/shared.module';
import { ChangeResearcherRejectFormComponent } from './change-researcher-reject-form/change-researcher-reject-form.component';
import { RequestListComponent } from './request-list/request-list.component';
import { RequestsRoutingModule } from './requests-routing.module';
import { RequestFormComponent } from './request-form/request-form.component';
import { RequestViewComponent } from './request-view/request-view.component';
import { ChangeResearcherAcceptFormComponent } from './change-researcher-accept-form/change-researcher-accept-form.component';
import { CaseSupportingDocumentRequestFormComponent } from './supporting-document-request-form/supporting-document-request-form.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { AttachedLetterRequestFormComponent } from './attached-letter-request-form/attached-letter-request-form.component';
// import { DatePickerHijriModule } from 'app/shared/components/date-picker-hijri/date-picker-hijri.module';
import { RequestDetailsComponent } from './request-details/request-details.component';
import { ChangeResearcherRequestDetailsComponent } from './change-researcher-request-details/change-researcher-request-details.component';
import { CaseSupportingDocumentRequestDetailsComponent } from './supporting-document-request-details/supporting-document-request-details.component';
import { CaseSupportingDocumentRequestItemFormComponent } from './supporting-document-request-item-form/supporting-document-request-item-form.component';
// import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CaseSupportingDocumentRequestReplyFormComponent } from './supporting-document-request-reply-form/supporting-document-request-reply-form.component';
import { HearingLegalMemoRequestDetailsComponent } from './hearing-legal-memo-request-details/hearing-legal-memo-request-details.component';
import { HearingLegalMemoRequestReplyFormComponent } from './hearing-legal-memo-request-reply-form/hearing-legal-memo-request-reply-form.component';
import { ExportCaseJudgmentRequestDetailsComponent } from './export-case-judgment-request-details/export-case-judgment-request-details.component';
import { ExportCaseJudgmentRequestFormComponent } from './export-case-judgment-request-form/export-case-judgment-request-form.component';
import { ExportCaseJudgmentRequestReplyFormComponent } from './eport-case-judgment-request-reply-form/export-case-judgment-request-reply-form.component';
import { CompareRequestWithHistoryFormComponent } from './compare-request-with-history/compare-request-with-history.component';
import { ObjectionPermitRequestDetailsComponent } from './objection-permit-request-details/objection-permit-request-details.component';
import { ObjectionPermitRequestReplyFormComponent } from './objection-permit-request-reply-form/objection-permit-request-reply-form.component';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { ExportCaseJudgmentApproveFormComponent } from './export-case-judgment-approve-form/export-case-judgment-approve-form.component';
import { CaseSupportingDocumentRequestApproveFormComponent } from './supporting-document-request-approve-form/supporting-document-request-approve-form.component';
import { ExportRequestFormComponent } from './export-request-form/export-request-form.component';
import { ConsultationSupportingDocumentFormComponent } from './consultation-request-form/consultation-request-form.component';
import { ConsultationSupportingDocumentDetailsComponent } from './consultation-request-details/consultation-request-details.component';
import { ConsultationSupportingDocumentReplyFormComponent } from './consultation-request-reply-form/consultation-request-reply-form.component';
import { ConsultationSupportingDocumentApproveFormComponent } from './consultation-request-approve-form/consultation-request-approve-form.component';
import { CasesModule } from '../cases/cases.module';
import { SupportingDocumentRequestWizardComponent } from './supporting-document-request-wizard/supporting-document-request-wizard.component';
import { TemplateImageComponent } from './supporting-document-request-wizard/template-image/template-image.component';
import { ObjectionPermitRequestFormComponent } from './objection-permit-request-form/objection-permit-request-form.component';
import { ObjectionLegalMemoRequestDetailsComponent } from './objection-legal-memo-request-details/objection-legal-memo-request-details.component';
import { ChangeHearingResearcherRequestFormComponent } from './change-hearing-researcher-request-form/change-hearing-researcher-request-form.component';
import { ChangeHearingResearcherRequestDetailsComponent } from './change-hearing-researcher-request-details/change-hearing-researcher-request-details.component';
import { ChangeHearingResearcherAcceptFormComponent } from './change-hearing-researcher-accept-form/change-hearing-researcher-accept-form.component';
import { ChangeHearingResearcherRejectFormComponent } from './change-hearing-researcher-reject-form/change-hearing-researcher-reject-form.component';
import { ObjectionLegalMemoRejectFormComponent } from './objection-legal-memo-reject-form/objection-legal-memo-reject-form.component';

@NgModule({
  declarations: [
    RequestListComponent,
    ChangeResearcherRejectFormComponent,
    RequestFormComponent,
    RequestViewComponent,
    ChangeResearcherAcceptFormComponent,
    CaseSupportingDocumentRequestFormComponent,
    AttachedLetterRequestFormComponent,
    RequestDetailsComponent,
    ChangeResearcherRequestDetailsComponent,
    CaseSupportingDocumentRequestDetailsComponent,
    CaseSupportingDocumentRequestItemFormComponent,
    CaseSupportingDocumentRequestReplyFormComponent,
    HearingLegalMemoRequestDetailsComponent,
    HearingLegalMemoRequestReplyFormComponent,
    ExportCaseJudgmentRequestDetailsComponent,
    ExportCaseJudgmentRequestFormComponent,
    ExportCaseJudgmentRequestReplyFormComponent,
    ObjectionPermitRequestDetailsComponent,
    ObjectionPermitRequestReplyFormComponent,
    CompareRequestWithHistoryFormComponent,
    ExportCaseJudgmentApproveFormComponent,
    CaseSupportingDocumentRequestApproveFormComponent,
    ExportRequestFormComponent,
    ConsultationSupportingDocumentFormComponent,
    ConsultationSupportingDocumentDetailsComponent,
    ConsultationSupportingDocumentReplyFormComponent,
    ConsultationSupportingDocumentApproveFormComponent,
    SupportingDocumentRequestWizardComponent,
    TemplateImageComponent,
    ObjectionPermitRequestFormComponent,
    ObjectionLegalMemoRequestDetailsComponent,
    ChangeHearingResearcherRequestFormComponent,
    ChangeHearingResearcherRequestDetailsComponent,
    ChangeHearingResearcherAcceptFormComponent,
    ChangeHearingResearcherRejectFormComponent,
    ObjectionLegalMemoRejectFormComponent
  ],
  imports: [
    SharedModule,
    RequestsRoutingModule,
    CKEditorModule,
    DatePickerDualModule,
    CasesModule
    // DatePickerHijriModule,
    // NgxDatatableModule

  ],
  providers: [DatePipe]
})
export class RequestsModule { }
