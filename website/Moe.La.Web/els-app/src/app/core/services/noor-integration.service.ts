import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class NoorIntegrationService extends DataService {

  constructor(http: HttpClient) {
    super(environment.API_URL + 'integration/noor', http);
  }
  getParty(searchText: string,investigationRecordId?:number|null) {    
    return this.http.get(Constants.API_URL + `integration/noor/find-party/${searchText}?investigationRecordId=${investigationRecordId}`);
  }
}