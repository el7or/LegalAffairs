import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'case-categories',
    loadChildren: () =>
      import('./case-categories/case-categories.module').then(
        (m) => m.CaseCategoriesModule
      ),
  },
  {
    path: 'case-ratings',
    loadChildren: () =>
      import('./case-ratings/case-ratings.module').then(
        (m) => m.CaseRatingsModule
      ),
  },
  {
    path: 'ministry-sectors',
    loadChildren: () =>
      import('./ministry-sectors/ministry-sectors.module').then((m) => m.MinistrySectorsModule),
  },
  {
    path: 'ministry-departments',
    loadChildren: () =>
      import('./ministry-departments/ministry-departments.module').then((m) => m.MinistryDepartmentsModule),
  },
  {
    path: 'cities',
    loadChildren: () =>
      import('./cities/cities.module').then((m) => m.CitiesModule),
  },
  {
    path: 'courts',
    loadChildren: () =>
      import('./courts/courts.module').then((m) => m.CourtsModule),
  },
  {
    path: 'provinces',
    loadChildren: () =>
      import('./provincies/provincies.module').then((m) => m.ProvinciesModule),
  },
  {
    path: 'field-mission-types',
    loadChildren: () =>
      import('./field-mission-types/field-mission-types.module').then((m) => m.FieldMissionTypesModule),
  },
  {
    path: 'job-titles',
    loadChildren: () =>
      import('./job-titles/job-titles.module').then((m) => m.JobTitlesModule),
  },
  {
    path: 'branches',
    loadChildren: () =>
      import('./branches/branches.module').then((m) => m.BranchesModule),
  },
  {
    path: 'department',
    loadChildren: () =>
      import('./departments/departments.module').then((m) => m.DepartmentsModule),
  },
  {
    path: 'party-types',
    loadChildren: () =>
      import('./party-types/party-types.module').then((m) => m.PartyTypesModule),
  },
  {
    path: 'identity-types',
    loadChildren: () =>
      import('./identity-types/identity-types.module').then((m) => m.IdentityTypesModule),
  },
  {
    path: 'attachment-types',
    loadChildren: () =>
      import('./attachment-types/attachment-types.module').then((m) => m.AttachmentTypesModule),
  },
  {
    path: 'investigation-questions',
    loadChildren: () =>
      import('./investigation-questions/investigation-questions.module').then((m) => m.InvestigationQuestionsModule),
  },
  {
    path: 'investigation-record-party-type',
    loadChildren: () =>
      import('./investigation-record-party-type/investigation-record-party-type.module').then((m) => m.InvestigationRecordPartyTypeModule),
  },
  {
    path: 'work-item-type',
    loadChildren: () =>
      import('./work-item-type/work-item-type.module').then((m) => m.WorkItemTypeModule),
  },
  {
    path: 'sub-work-item-type',
    loadChildren: () =>
      import('./sub-work-item-type/sub-work-item-type.module').then((m) => m.SubWorkItemTypeModule),
  },
  {
    path: 'cases-for-pleading',
    loadChildren: () =>
      import('./cases-received-for-pleading/cases-received-for-pleading-routing.module').then((m) => m.CasesReceivedForPleadingRoutingModule),
  },
  {
    path: 'district',
    loadChildren: () =>
      import('./districts/districts.module').then((m) => m.DistrictsModule),
  },
  {
    path: 'governmentOrganization',
    loadChildren: () =>
      import('./governmentOrganizations/governmentOrganizations.module').then((m) => m.GovernmentOrganizationsModule),
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrationManagementRoutingModule { }
