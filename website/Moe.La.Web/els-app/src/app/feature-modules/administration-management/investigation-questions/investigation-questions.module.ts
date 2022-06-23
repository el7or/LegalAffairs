import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { InvestigationQuestionsRoutingModule } from './investigation-questions-routing.module';
import { InvestigationQuestionListComponent } from './investigation-question-list/investigation-question-list.component';
import { InvestigationQuestionFormComponent } from './investigation-question-form/investigation-question-form.component';
import { SharedModule } from 'app/shared/shared.module';
import { InvestigationQuestionStatusFormComponent } from './investigation-question-status-form/investigation-question-status-form.component';


@NgModule({
  declarations: [InvestigationQuestionListComponent, InvestigationQuestionFormComponent, InvestigationQuestionStatusFormComponent],
  imports: [
    SharedModule,
    InvestigationQuestionsRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class InvestigationQuestionsModule { }
