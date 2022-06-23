import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { QueryObject } from '../models/query-objects';

@Injectable({
  providedIn: 'root',
})
export class InvestigationService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.INVESTIGATION_API_URL, http);
  }

  exportPdf(queryObject: QueryObject) {
    return this.http.get(
      `${Constants.INVESTIGATION_API_URL}/export-pdf?` +
        this.toQueryString(queryObject),
      {
        responseType: 'blob' as 'json',
      }
    );
  }

  exportExcel(queryObject: QueryObject) {
    return this.http.get(
      `${Constants.INVESTIGATION_API_URL}/export-excel?` +
        this.toQueryString(queryObject),
      {
        responseType: 'blob' as 'json',
      }
    );
  }
}
