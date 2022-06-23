import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class BranchService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.BRANCH_API_URL, http);

  }

  getByName(name: any) {
    return this.http.get(Constants.BRANCH_API_URL + '/name/' + name);
  }

  /**
   * Get the related departments for a given general management.
   * @param id The general management Id.
   */
  getDepartments(id: number) {
    return this.http.get(`${this.url}/${id}/departments`);
  }
  
  isNameExists(branch: any) {
    return this.http.post(Constants.BRANCH_API_URL + '/is-name-exists', branch);
  }
}
