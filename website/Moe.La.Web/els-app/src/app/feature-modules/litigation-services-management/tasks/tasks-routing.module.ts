import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TasksWaitingComponent } from './tasks-waiting/tasks-waiting.component';
import { TasksCompletedComponent } from './tasks-completed/tasks-completed.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'waiting',
    pathMatch: 'full',
  },
  {
    path: 'waiting',
    component: TasksWaitingComponent,
    data: {
      title: 'مهام غير منجزة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'مهام غير منجزة' }],
    },
  },
  {
    path: 'completed',
    component: TasksCompletedComponent,
    data: {
      title: 'مهام منجزة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'مهام منجزة' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TasksRoutingModule {}
