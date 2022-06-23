import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { CaseRatingsRoutingModule } from './case-ratings-routing.module';
import { CaseRatingListComponent } from './case-rating-list/case-rating-list.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CaseRatingFormComponent } from './case-rating-form/case-rating-form.component';

@NgModule({
  declarations: [CaseRatingListComponent, CaseRatingFormComponent],
  imports: [
    SharedModule,
    CaseRatingsRoutingModule,
    SweetAlert2Module.forChild(),
    ReactiveFormsModule,
    FormsModule,
  ]
})
export class CaseRatingsModule { }
