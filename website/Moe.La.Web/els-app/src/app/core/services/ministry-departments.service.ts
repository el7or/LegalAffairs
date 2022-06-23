import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class MinistryDepartmentService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.MINISTRYDEPARTMENT_API_URL , http);
  }

  isNameExists(ministryDepartment: KeyValuePairs) {
    return this.http.post(Constants.MINISTRYDEPARTMENT_API_URL + '/is-name-exists', ministryDepartment);
  }
}
