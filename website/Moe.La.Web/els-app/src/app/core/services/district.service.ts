import { Injectable } from '@angular/core';
import { DataService } from './data.service'; 
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants'; 
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({
  providedIn: 'root',
})
export class DistrictService extends DataService { 

  constructor(http: HttpClient) {
    super(Constants.DISTRICT_API_URL, http);
  }

  isNameExists(district: any) {
    return this.http.post(Constants.DISTRICT_API_URL + '/is-name-exists', district);
  }
}
