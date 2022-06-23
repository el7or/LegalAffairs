import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;

import { DataService } from './data.service';
import { CustomerQueryObject } from '../models/query-objects';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService extends DataService {

  queryObject: CustomerQueryObject = {
    partyTypeId: null,
    provinceId: null,
    cityId: null,
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 20
  };

  constructor(http: HttpClient) {
    super(environment.API_URL + 'customers', http);
  }

  getProvinces() {
    return super.getAll(environment.API_URL + 'provinces?pageSize=100');
  }

  getCities(provinceId) {
    return super.getAll(environment.API_URL + 'cities?pageSize=300&provinceId=' + provinceId);
  }

  getAgencies(customerId) {
    return super.getAll(environment.API_URL + 'agencies?pageSize=300&customerId=' + customerId);
  }
}
