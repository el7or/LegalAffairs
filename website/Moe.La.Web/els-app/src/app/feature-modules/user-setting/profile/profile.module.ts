import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileViewComponent } from './profile-view/profile-view.component';

@NgModule({
  declarations: [ProfileViewComponent],
  imports: [
    SharedModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
