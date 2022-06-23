import { Injectable } from '@angular/core';
import { extend } from 'webdriver-js-extender';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class RoleService extends DataService{

  constructor(http: HttpClient) {
    super(environment.API_URL + 'roles', http);
  }

}
