import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class DepartmentService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.DEPARTMENT_API_URL, http);
  }

  isNameExists(name: string) {
    return this.http.get(Constants.DEPARTMENT_API_URL + '/is-name-exists/' + name);
  }

}
