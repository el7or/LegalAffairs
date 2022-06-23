import { Routes } from '@angular/router';

import { ErrorComponent } from './error/error.component';
import { LoginComponent } from './login/login.component';
export const AuthRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '404',
        component: ErrorComponent
      },
      {
        path: 'login',
        component: LoginComponent,
        data: { title: 'تسجيل الدخول' },
      }
    ]
  }
];
