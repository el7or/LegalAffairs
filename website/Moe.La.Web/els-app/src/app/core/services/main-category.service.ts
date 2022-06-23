import { Injectable } from '@angular/core';
import { DataService } from './data.service'; 
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants'; 
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({
  providedIn: 'root',
})
export class MainCategoryService extends DataService { 

  constructor(http: HttpClient) {
    super(Constants.MAIN_CATEGORY_API_URL, http);
  }
  
  isNameExists(caseCategory: KeyValuePairs) {
    return this.http.post(Constants.MAIN_CATEGORY_API_URL + '/is-name-exists', caseCategory);
  }
}
