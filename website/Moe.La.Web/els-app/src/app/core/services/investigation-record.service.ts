import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class InvestigationRecordService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.INVESTIGATION_RECORD_API_URL, http);
  }
}
