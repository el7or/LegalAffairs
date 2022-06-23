import { NgModule, ErrorHandler, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from './shared/shared.module';
import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { FullComponent } from './layouts/full/full.component';
import { AppBlankComponent } from './layouts/blank/blank.component';
import { AppHeaderComponent } from './layouts/full/header/header.component';
import { AppSidebarComponent } from './layouts/full/sidebar/sidebar.component';
import { AppBreadcrumbComponent } from './layouts/full/breadcrumb/breadcrumb.component';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { SpinnerComponent } from './shared/spinner.component';
import { TokenInterceptorProvidor } from './core/services/token-interceptor';
import { getArabicPaginatorIntl } from './shared/paginator/arabic-paginator-intl';
import { AppErrorHandler } from './shared/errors/app-error-handler';
import { ErrorService } from './core/services/error.service';
import { GlobalHttpInterceptorService } from './shared/errors/global-http-interceptor.service';
import { AuthCommunicatorService } from './core/services/auth-communicator.service';

export function initApp(authCommunicatorService: AuthCommunicatorService) {
  return () => authCommunicatorService.init();
}

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
  wheelSpeed: 2,
  wheelPropagation: true,
};

@NgModule({
  declarations: [
    AppComponent,
    FullComponent,
    AppHeaderComponent,
    SpinnerComponent,
    AppBlankComponent,
    AppSidebarComponent,
    AppBreadcrumbComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    PerfectScrollbarModule,
    RouterModule.forRoot(AppRoutes, { relativeLinkResolution: 'legacy' }),
    SweetAlert2Module.forRoot()
    
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
    { provide: MatPaginatorIntl, useValue: getArabicPaginatorIntl() },
    { provide: MAT_DATE_LOCALE, useValue: 'ar-SA' },
    { provide: HTTP_INTERCEPTORS, useClass: GlobalHttpInterceptorService, multi: true },
    {  // it will not show the errors in the html and the page just will be hung
      provide: ErrorHandler,
      useClass: AppErrorHandler,
    },

    ErrorService,
    TokenInterceptorProvidor,
    AuthCommunicatorService,
    {
      provide: APP_INITIALIZER,
      useFactory: initApp,
      deps: [AuthCommunicatorService],
      multi: true
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
