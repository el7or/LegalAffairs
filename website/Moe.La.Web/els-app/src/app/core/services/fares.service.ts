import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class FaresService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.FARES_API_URL , http);
  }

  getUser(id: string) {
    return this.http.get(Constants.API_URL + `integration/faris/user/${id}`);
  }
  getParty(searchText: string,investigationRecordId?:number|null) {
    return this.http.get(Constants.API_URL + `integration/faris/find-party/${searchText}?investigationRecordId=${investigationRecordId}`);
  }
}
