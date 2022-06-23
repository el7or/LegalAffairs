import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { GovernmentOrganizationsRoutingModule } from './governmentOrganizations-routing.module';
import { GovernmentOrganizationListComponent } from './governmentOrganization-list/governmentOrganization-list.component';
import { GovernmentOrganizationFormComponent } from './governmentOrganization-form/governmentOrganization-form.component';

@NgModule({
  declarations: [GovernmentOrganizationFormComponent, GovernmentOrganizationListComponent],
  imports: [
    SharedModule,
    GovernmentOrganizationsRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class GovernmentOrganizationsModule { }
