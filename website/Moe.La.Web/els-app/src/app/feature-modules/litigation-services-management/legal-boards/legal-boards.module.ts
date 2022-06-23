import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { LegalBoardFormComponent } from './legal-board-form/legal-board-form.component';
import { LegalBoardListComponent } from './legal-board-list/legal-board-list.component';
import { LegalBoardsListRoutingModule } from './legal-boards-routing.module';
import { MemberDetailsComponent } from './member-details/member-details.component';
import { MemberFormComponent } from './member-form/member-form.component';
import { LegalBoardViewComponent } from './legal-board-view/legal-board-view.component';

@NgModule({
  declarations: [
    LegalBoardListComponent,
    LegalBoardFormComponent,
    MemberDetailsComponent,
    MemberFormComponent,
    LegalBoardViewComponent
  ],
  imports: [
    SharedModule,
    LegalBoardsListRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class LegalBoardsModule { }
