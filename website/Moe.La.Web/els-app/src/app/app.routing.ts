import { AuthGuard } from './core/guards/auth.guard';
import { FeatureModulesModule } from './feature-modules/feature-modules.module';
import { Routes } from '@angular/router';

import { FullComponent } from './layouts/full/full.component';
import { AppBlankComponent } from './layouts/blank/blank.component';

export const AppRoutes: Routes = [
  {
    path: '',
    component: FullComponent,
    canActivate: [AuthGuard],
    runGuardsAndResolvers: 'always',
    children: [
      {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full',
      },
      {
        path: 'home',
        redirectTo: '/home',
        pathMatch: 'full',
      },
      {
        path: '',
        loadChildren: () =>
          import('./feature-modules/feature-modules.module').then(
            (m) => m.FeatureModulesModule
          ),
      },
    ],
  },
  {
    path: '',
    component: AppBlankComponent,
    children: [
      {
        path: 'auth',
        loadChildren: () =>
          import('./auth/auth.module').then((m) => m.AuthModule),
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'auth/404',
  },
];
