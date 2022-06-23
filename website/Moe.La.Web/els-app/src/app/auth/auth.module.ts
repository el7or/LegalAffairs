import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
    MatIconModule
} from '@angular/material/icon';
import {
    MatCardModule,
} from '@angular/material/card';
import {
    MatInputModule
} from '@angular/material/input';
import {
    MatCheckboxModule
} from '@angular/material/checkbox';
import {
    MatButtonModule
} from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AuthRoutes } from './auth.routing';
import { ErrorComponent } from './error/error.component';
import { LoginComponent } from './login/login.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(AuthRoutes),
        MatIconModule,
        MatCardModule,
        MatInputModule,
        MatCheckboxModule,
        MatButtonModule,
        FlexLayoutModule,
        FormsModule,
        ReactiveFormsModule,
        MatProgressSpinnerModule
    ],
    declarations: [
        ErrorComponent,
        LoginComponent,
    ]
})
export class AuthModule { }
