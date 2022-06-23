import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { EnumValue } from '../models/enum-value';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class CourtService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.COURT_API_URL, http);
  }

  getCourtTypes() {
    return this.http.get(Constants.COURT_API_URL + '/litigation-types');
  }

  getCourtCategories() {
    return this.http.get(Constants.COURT_API_URL + '/court-categories');
  }
  
  isNameExists(court: any) {
    return this.http.post(Constants.COURT_API_URL + '/is-name-exists', court);
  }
}
