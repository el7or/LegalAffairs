import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { CaseCategoriesRoutingModule } from './case-categories-routing.module';
import { CaseCategoryListComponent } from './case-category-list/case-category-list.component';
import { CaseCategoryFormComponent } from './case-category-form/case-category-form.component';
import { CaseSecondSubCategoryFormComponent } from './case-second-sub-category-form/case-second-sub-category-form.component';

@NgModule({
  declarations: [
    CaseCategoryListComponent,
     CaseCategoryFormComponent,
     CaseSecondSubCategoryFormComponent
    ],
  imports: [
    SharedModule,
    CaseCategoriesRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class CaseCategoriesModule { }
