import { Injectable } from '@angular/core';
import { DataService } from './data.service'; 
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants'; 
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({
  providedIn: 'root',
})
export class SecondSubCategoryService extends DataService { 

  constructor(http: HttpClient) {
    super(Constants.SECOND_SUB_CATEGORY_API_URL, http);
  }
  
  isNameExists(caseCategory: KeyValuePairs) {
    return this.http.post(Constants.SECOND_SUB_CATEGORY_API_URL + '/is-name-exists', caseCategory);
  }

  changeCatergoryActivity(SecondSubCategoryId:number,IsActive:boolean){
    return this.http.put(Constants.SECOND_SUB_CATEGORY_API_URL + '/change-category-activity?SecondSubCategoryId='+SecondSubCategoryId+'&&IsActive='+IsActive,{});
  }
}
