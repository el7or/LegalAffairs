import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;
import { DataService } from './data.service';
import { Constants } from '../constants';
import { KeyValuePairs } from '../models/key-value-pairs';

@Injectable({
  providedIn: 'root'
})
export class PartyTypeService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.PARTYTYPE_API_URL, http);
  }

  isNameExists(partyType: KeyValuePairs) {
    return this.http.post(Constants.PARTYTYPE_API_URL + '/is-name-exists', partyType);
  }
}
