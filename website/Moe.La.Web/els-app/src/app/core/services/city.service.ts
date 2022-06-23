import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class CityService extends DataService {

  constructor(http: HttpClient) {
    super(environment.API_URL + 'cities', http);
  }

  async getByName(name:any): Promise<any> {
    return await this.http.get(environment.API_URL + 'cities/name/' + name);
  }

  getProvinces() {
    return super.getAll(environment.API_URL + 'provinces');
  }

  isNameExists(city:any) {
    return this.http.post(environment.API_URL + 'cities/is-name-exists', city);
  }

}
