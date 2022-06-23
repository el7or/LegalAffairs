import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

import { SharedModule } from 'app/shared/shared.module';
import { LegalMemosRoutingModule } from './legal-memos-routing.module';
import { LegalMemoFormComponent } from './legal-memo-form/legal-memo-form.component';
import { LegalMemoListComponent } from './legal-memo-list/legal-memo-list.component';
import { LegalMemoViewComponent } from './legal-memo-view/legal-memo-view.component';
import { MemoNoteFormComponent } from './memo-note-form/memo-note-form.component';
import { LegalMemoReviewListComponent } from './legal-memo-review-list/legal-memo-review-list.component';
import { LegalMemoBoardReviewListComponent } from './legal-memo-board-review-list/legal-memo-board-review-list.component';
import { AssignMemoToBoardFormComponent } from './assign-memo-to-board-form/assign-memo-to-board-form.component';
import { CasesModule } from '../cases/cases.module';
import { DeleteLegalMemoComponent } from './delete-legal-memo/delete-legal-memo.component';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { CaseHearingSharedModule } from '../caseHearingShared/cases-hearing-shared.module';
import { BoardMeetingComponent } from './board-meeting/board-meeting.component';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { MemoRejectFormComponent } from './memo-reject-form/memo-reject-form.component';
import { BoardMeetingListComponent } from './board-meeting-list/board-meeting-list.component';
import { BoardMeetingViewComponent } from './board-meeting-view/board-meeting-view.component';

@NgModule({
  declarations: [
    MemoRejectFormComponent,
    LegalMemoFormComponent,
    LegalMemoListComponent,
    LegalMemoViewComponent,
    MemoNoteFormComponent,
    LegalMemoReviewListComponent,
    LegalMemoBoardReviewListComponent,
    AssignMemoToBoardFormComponent,
    DeleteLegalMemoComponent,
    BoardMeetingComponent,
    BoardMeetingListComponent,
    BoardMeetingViewComponent
  ],
  imports: [
    SharedModule,
    LegalMemosRoutingModule,
    SweetAlert2Module.forChild(),
    TimepickerModule.forRoot(),
    CKEditorModule,
    CasesModule,
    DatePickerDualModule,
    CaseHearingSharedModule
  ],
  providers: [DatePipe],
})
export class LegalMemosModule { }
