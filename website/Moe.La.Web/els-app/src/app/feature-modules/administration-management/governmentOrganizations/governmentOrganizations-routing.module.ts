import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GovernmentOrganizationListComponent } from './governmentOrganization-list/governmentOrganization-list.component';

const routes: Routes = [{
  path: '',
  component: GovernmentOrganizationListComponent,
  data: {
    title: 'الجهات',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الجهات' }],
  },
},];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GovernmentOrganizationsRoutingModule { }
