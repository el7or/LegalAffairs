import { Injectable } from '@angular/core';
import { extend } from 'webdriver-js-extender';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({ providedIn: 'root' })
export class WorkItemTypeService extends DataService{

  constructor(http: HttpClient) {
    super(Constants.WORK_ITEM_TYPE_API_URL , http);
  }

  isNameExists(workItemType: any) {
    return this.http.post(Constants.WORK_ITEM_TYPE_API_URL + '/is-name-exists', workItemType);
  }
}