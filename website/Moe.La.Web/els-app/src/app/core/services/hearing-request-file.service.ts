import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { EnumValue } from '../models/enum-value';
import { HearingRequestFileQueryObject } from '../models/query-objects';
import { environment } from '../../../environments/environment';

@Injectable()
export class HearingRequestFileService extends DataService {

  queryObject: HearingRequestFileQueryObject = {
    hearingId: null,
    caseId: null,
    currentUserId: null,
    closed: null,
    sortBy: 'requestDate',
    isSortAscending: false,
    page: 1,
    pageSize: 20
  };

  constructor(http: HttpClient) {
    super(environment.API_URL + 'hearings/request-files', http);
  }

  async getNextId(): Promise<number> {

    let id: number;
    await this.http.get(environment.API_URL + 'hearings/request-files/next-id');

    return id;
  }
}
