import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CaseRatingListComponent} from './case-rating-list/case-rating-list.component';

const routes: Routes = [
  {
    path: '',
    component: CaseRatingListComponent,
    data: {
      title: 'التقييمات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'التقييمات' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseRatingsRoutingModule { }
