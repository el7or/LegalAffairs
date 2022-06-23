import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvestigationRecordPartyTypeService extends DataService {

  constructor(http: HttpClient) {
    super(environment.API_URL + 'investigation-record-party-type', http);
  }

  isNameExists(name: string) {
    return this.http.get(environment.API_URL + 'investigation-record-party-type/is-name-exists/' + name);
  }
}
