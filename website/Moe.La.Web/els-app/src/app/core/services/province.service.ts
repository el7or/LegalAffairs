import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class ProvinceService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.PROVINCE_API_URL, http);
  }

  getAllWithCities() {
    return this.http.get(Constants.PROVINCE_API_URL + '/with-cities')
  }

  isNameExists(name: string) {
    return this.http.get(Constants.PROVINCE_API_URL + '/is-name-exists/' + name);
  }
}
