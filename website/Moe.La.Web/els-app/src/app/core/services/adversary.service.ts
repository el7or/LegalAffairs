import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { EnumValue } from '../models/enum-value';
import { AdversaryQueryObject } from '../models/query-objects';
import { environment } from '../../../environments/environment';

@Injectable()
export class AdversaryService extends DataService {

  queryObject: AdversaryQueryObject = {
    categoryId: null,
    provinceId: null,
    cityId: null,
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 20
  };

  constructor(http: HttpClient) {
    super(environment.API_URL + 'adversaries', http);
  }

  //async getCategories()    {
  //  return await this.http.get(environment.API_URL + 'adversaries/categories')
  //      
  //      
  //    .then(result => {
  //      return result;
  //    });
  //}

  //categoriesTranslator = [
  //  { en: "Individual", ar: "فرد" },
  //  { en: "Establishment", ar: "مؤسسة" },
  //  { en: "Company", ar: "شركة" }
  //]

  //getArTypeName(enName) {
  //  if (this.categoriesTranslator.find(t => t.en == enName))
  //    return this.categoriesTranslator.find(t => t.en == enName).ar;
  //}

  getProvinces() {
    return super.getAll(environment.API_URL + 'provinces?pageSize=100');
  }

  getCities(provinceId) {
    return super.getAll(environment.API_URL + 'cities?pageSize=300&provinceId=' + provinceId);
  }

}
