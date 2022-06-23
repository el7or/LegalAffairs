import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { CaseChangeConsultantRequestQueryObject } from '../models/query-objects';
import { environment } from '../../../environments/environment';

@Injectable()
export class CaseChangeConsultantRequestService extends DataService {

  queryObject: CaseChangeConsultantRequestQueryObject = {
    caseId: null,
    addUserId: null,
    isAccept: null,
    sortBy: 'requestDate',
    isSortAscending: false,
    page: 1,
    pageSize: 20
  };

  constructor(http: HttpClient) {
    super(environment.API_URL + 'cases/consultant-requests', http);
  }

}
