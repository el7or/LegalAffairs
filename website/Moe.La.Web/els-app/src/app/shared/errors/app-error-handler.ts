import { ErrorHandler, Injectable, Injector, NgZone, isDevMode } from '@angular/core';
import { ErrorService } from '../../core/services/error.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AppError } from './app-error';
import { AlertService } from 'app/core/services/alert.service';

@Injectable()
export class AppErrorHandler implements ErrorHandler {

  constructor(
    private injector: Injector,
    private errorService: ErrorService,
    private alert: AlertService,
    private ngZone: NgZone) {
  }

  handleError(error: any) {

    let errorURL = 'Error URL: ' + this.injector.get(Router).url;

    let errorType = "";
    let errorMessage = error.message;

    if (error instanceof HttpErrorResponse) {
      errorType = 'There was an HTTP error. \n Status code:' + (<HttpErrorResponse>error).status;
    }
    else if (error instanceof TypeError) {
      errorType = 'There was a type error.';
    }
    else if (error instanceof Error) {
      errorType = 'There was a general error.';
    }
    else if (error instanceof AppError) {
      errorType = 'There was an application error.';
      errorMessage = errorMessage ? errorMessage : error.originalError.statusText;
    }
    else {
      errorType = 'Error!';
      errorMessage = error.message ? error.message : 'An unexpected error happened!';
    }

    this.ngZone.run(() => {
      console.error(errorType, errorMessage);
      //this.alert.error(errorType + ' ' + errorMessage);
    });

    if (!isDevMode)
      this.errorService.logError(`${errorURL} %0D%0A${errorType} %0D%0A${errorMessage}`);
    else
      throw error;
  }

}
