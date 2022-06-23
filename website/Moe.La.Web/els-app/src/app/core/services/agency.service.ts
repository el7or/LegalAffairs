import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;

import { DataService } from './data.service';
import { AgencyQueryObject } from '../models/query-objects';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AgencyService extends DataService{
  queryObject: AgencyQueryObject = {
    customerId: null,
    enabled: null,
    sortBy: 'customer',
    isSortAscending: true,
    page: 1,
    pageSize: 20
  };

  constructor(http: HttpClient) {
    super(environment.API_URL + 'agencies', http);
  }

  getCustomers() {
    return super.getAll(environment.API_URL + 'customers?pageSize=9999');
  }
}
