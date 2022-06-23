import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CaseCategoryListComponent } from './case-category-list/case-category-list.component';

const routes: Routes = [
  {
    path: '',
    component: CaseCategoryListComponent,
    data: {
      title: 'تصنيف القضية',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تصنيف القضية' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseCategoriesRoutingModule { }
