import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'memos',
    loadChildren: () =>
      import('./legal-memos/legal-memos.module').then(
        (m) => m.LegalMemosModule
      ),
  },
  {
    path: 'cases',
    loadChildren: () =>
      import('./cases/cases.module').then((m) => m.CasesModule),
  },
  {
    path: 'hearings',
    loadChildren: () =>
      import('./hearings/hearings.module').then((m) => m.HearingsModule),
  },
  {
    path: 'requests',
    loadChildren: () =>
      import('./requests/requests.module').then((m) => m.RequestsModule),
  },
  {
    path: 'tasks',
    loadChildren: () =>
      import('./tasks/tasks.module').then((m) => m.TasksModule),
  },
  {
    path: 'parties',
    loadChildren: () =>
      import('./parties/parties.module').then((m) => m.PartiesModule),
  },
  {
    path: 'legal-boards',
    loadChildren: () =>
      import('./legal-boards/legal-boards.module').then(
        (m) => m.LegalBoardsModule
      ),
  },
  // {
  //   path: 'moamalat',
  //   loadChildren:() =>
  //   import('./moamalat-transactions/moamalat-transactions.module').then((m) => m.MoamalatTransactionsModule),
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LitigationServicesManagementRoutingModule { }
