import { Injectable } from '@angular/core';
import { extend } from 'webdriver-js-extender';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class SubWorkItemTypeService extends DataService{

  constructor(http: HttpClient) {
    super(Constants.SUB_WORK_ITEM_TYPE_API_URL , http);
  }

  isNameExists(subWorkItemType: any) {
    return this.http.post(Constants.SUB_WORK_ITEM_TYPE_API_URL + '/is-name-exists', subWorkItemType);
  }

}