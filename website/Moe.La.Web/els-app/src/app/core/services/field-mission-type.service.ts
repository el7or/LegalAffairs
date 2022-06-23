import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({
  providedIn: 'root'
})
export class FieldMissionTypeService extends DataService{

  constructor(http: HttpClient) {
    super(environment.API_URL + 'field-mission-types', http);
  }

  isNameExists(fieldMissionType: any) {
    return this.http.post(environment.API_URL + 'field-mission-types/is-name-exists', fieldMissionType);
  }
}
