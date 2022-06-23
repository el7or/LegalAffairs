import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { TasksRoutingModule } from './tasks-routing.module';

@NgModule({
  declarations: [],
  imports: [
    SharedModule,
    TasksRoutingModule
  ]
})
export class TasksModule { }
