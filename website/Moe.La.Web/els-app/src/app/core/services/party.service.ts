import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { CaseListItem } from '../models/case';
import { EnumValue } from '../models/enum-value';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { Guid } from 'guid-typescript';
import { CaseStatus } from '../enums/CaseStatus';
import { KeyValuePairs } from '../models/key-value-pairs';
import { CaseQueryObject } from '../models/query-objects';

@Injectable({
  providedIn: 'root'
})
export class PartyService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.PARTY_API_URL, http);
  }

  isPartyExist(party: any) {
    return this.http.post(
      `${Constants.PARTY_API_URL}/party-exist`,
      party
    );
  }
}
