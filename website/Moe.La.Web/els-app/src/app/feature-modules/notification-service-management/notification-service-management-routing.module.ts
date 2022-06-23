import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotificationListComponent } from './notification-list/notification-list.component';

const routes: Routes = [
  {
    path: '',
    component: NotificationListComponent,
    data: {
      title: 'الاشعارات ',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الاشعارات' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationServiceManagementRoutingModule { }
