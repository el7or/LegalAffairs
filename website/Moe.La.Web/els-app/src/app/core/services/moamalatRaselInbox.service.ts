import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class MoamalatRaselInboxService  extends DataService {
  constructor(http: HttpClient) {
    super(Constants.MOAMALAT_RASEL_INBOX_API_URL, http);
  }

  receiveMoamalaRasel(id: number) {
    return this.http.put(`${Constants.MOAMALAT_RASEL_INBOX_API_URL}/${id}`,'');
  }
  
}
