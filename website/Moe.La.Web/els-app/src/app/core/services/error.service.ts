import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';

@Injectable()
export class ErrorService {

  constructor(private http: HttpClient) {
  }

  logError(msg: string) {
    this.http.post(environment.API_URL + 'error/logging?errorMessage=' + msg, null).subscribe(() => {
    });
  }
}
