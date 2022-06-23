import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MinistrySectorListComponent } from './ministry-sectors-list/ministry-sectors-list.component';


const routes: Routes = [
  {
    path: '',
    component: MinistrySectorListComponent,
    data: {
      title: 'قطاعات الوزارة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'قطاعات الوزارة' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MinistrySectorsRoutingModule { }
