import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProfileViewComponent } from './profile-view/profile-view.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'profile',
    pathMatch: 'full',
  },
  {
    path: 'profile',
    component: ProfileViewComponent,
    data: {
      title: 'الملف الشخصى',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الملف الشخصى' }],
    },
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
