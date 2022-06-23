import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InvestigationQuestionListComponent } from './investigation-question-list/investigation-question-list.component';

const routes: Routes = [
  {
    path: '',
    component: InvestigationQuestionListComponent,
    data: {
      title: 'أسئلة التحقيقات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أسئلة التحقيقات' }],
    },
  },
  // {
  //   path: '',
  //   component: InvestigationQuestionFormComponent,
  //   data: {
  //     title: 'إضافة سؤال تحقيق',
  //     urls: [{ title: 'الرئيسية', url: '/' }, { title: 'إضافة سؤال'}],
  //   },
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvestigationQuestionsRoutingModule { }
