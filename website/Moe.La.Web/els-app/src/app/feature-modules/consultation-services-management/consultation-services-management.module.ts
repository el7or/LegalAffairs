import { MoamalatServicesManagementModule } from './../moamalat-services-management/moamalat-services-management.module';
import { SharedModule } from './../../shared/shared.module';
import { NgModule } from '@angular/core';

import { ConsultationServicesManagementRoutingModule } from './consultation-services-management-routing.module';
import { ConsultationMoamalaDetailsComponent } from './consultation-moamala-details/consultation-moamala-details.component';
import { ConsultationFormRegulationsAndLawsComponent } from './consultation-form-regulations-and-laws/consultation-form-regulations-and-laws.component';
import { ConsultationFormComponent } from './consultation-form/consultation-form.component';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { ConsultationListComponent } from './consultation-list/consultation-list.component';
import { ConsultationReviewViewComponent } from './consultation-review-view/consultation-review-view.component';
import { ConsultationReviewFormComponent } from './consultation-review-form/consultation-review-form.component';
import { ConsultationTransactionListComponent } from './consultation-transaction-list/consultation-transaction-list.component';
import { ConsultationGroundsListComponent } from './consultation-grounds-list/consultation-grounds-list.component';
import { ConsultationMeritsListComponent } from './consultation-merits-list/consultation-merits-list.component';
import { ConsultationVisualListComponent } from './consultation-visual-list/consultation-visual-list.component';
import { RelatedMoamalatListComponent } from './related-moamalat-list/related-moamalat-list.component';

@NgModule({
  declarations: [ConsultationMoamalaDetailsComponent, ConsultationFormRegulationsAndLawsComponent, ConsultationFormComponent, ConsultationListComponent, ConsultationReviewViewComponent, ConsultationReviewFormComponent, ConsultationTransactionListComponent, ConsultationGroundsListComponent, ConsultationMeritsListComponent, ConsultationVisualListComponent, RelatedMoamalatListComponent],
  imports: [
    SharedModule,
    ConsultationServicesManagementRoutingModule,
    DatePickerDualModule,
    MoamalatServicesManagementModule
  ]
})
export class ConsultationServicesManagementModule { }
