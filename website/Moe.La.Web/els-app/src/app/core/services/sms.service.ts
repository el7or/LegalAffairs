import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';

@Injectable()
export class SmsService extends DataService {

  constructor(http: HttpClient) {
    super(environment.API_URL + 'sms', http);
  }

  send(msg: string, numbers: string) {
    return this.http.post(environment.API_URL + 'sms/' + msg + "/" + numbers, {}) ;
  }

}
