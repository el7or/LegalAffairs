import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FeatureModulesComponent } from './feature-modules.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'home',
        component: FeatureModulesComponent,
        data: {
          title: 'الرئيسية',
          //   urls: [{ title: 'الرئيسية', url: '/' }],
        },
      },
      {
        path: '',
        loadChildren: () =>
          import('./litigation-services-management/litigation-services-management.module').then(
            (m) => m.LitigationServicesManagementModule
          ),
      },

      {
        path: 'reports',
        loadChildren: () =>
          import(
            './litigation-services-management/reports/reports.module'
          ).then((m) => m.ReportsModule),
      },
      {
        path: 'pleading/cases-for-pleading',
        loadChildren: () =>
          import('./administration-management/cases-received-for-pleading/cases-received-for-pleading.module').then(
            (m) => m.CasesReceivedForPleadingModule
          ),
      },
      {
        path: 'pleading/researchers-consultants',
        loadChildren: () =>
          import('./administration-management/researchers-consultants/researchers-consultants.module').then((m) => m.ResearchersConsultantsModule),
      },
      {
        path: 'administration-management',
        loadChildren: () =>
          import(
            './administration-management/administration-management.module'
          ).then((m) => m.AdministrationManagementModule),
      },
      {
        path: 'users',
        loadChildren: () =>
          import('./administration-management/users/users.module').then(
            (m) => m.UsersModule
          ),
      },
      {
        path: 'workflow-settings',
        loadChildren: () =>
          import('./administration-management/workflow-settings/workflow-settings.module').then(
            (m) => m.WorkflowSettingsModule
          ),
      },
      {
        path: 'user-settings',
        loadChildren: () =>
          import('./user-setting/user-setting.module').then(
            (m) => m.UserSettingModule
          ),
      },
      {
        path: 'investigation-services-management',
        loadChildren: () =>
          import('./investigation-services-management/investigation-services-management.module').then(
            (m) => m.InvestigationServicesManagementModule
          ),
      },
      {
        path: 'investigation-records',
        loadChildren: () =>
          import('./investigation-services-management/investigation-records/investigation-records.module').then(
            (m) => m.InvestigationRecordsModule
          ),
      },
      {
        path: 'investigations',
        loadChildren: () =>
          import('./investigation-services-management/investigations/investigations.module').then(
            (m) => m.InvestigationsModule
          ),
      },
      {
        path: 'moamalat',
        loadChildren: () =>
          import('./moamalat-services-management/moamalat-services-management.module').then(
            (m) => m.MoamalatServicesManagementModule
          ),
      }
      ,
      {
        path: 'moamala-rasel-inbox',
        loadChildren: () =>
          import('./moamalat-services-management/moamala-rasel-inbox/moamala-rasel-inbox.module').then(
            (m) => m.MoamalaRaselInboxModule
          ),
      },
      {
        path: 'consultation',
        loadChildren: () =>
          import('./consultation-services-management/consultation-services-management.module').then(
            (m) => m.ConsultationServicesManagementModule
          ),
      },
      {
        path: 'notifications',
        loadChildren: () =>
          import('./notification-service-management/notification-service-management.module').then(
            (m) => m.NotificationServiceManagementModule
          ),
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FeatureModulesRoutingModule { }
