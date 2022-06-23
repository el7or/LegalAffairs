import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class MinistrySectorService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.MINISTRYSECTOR_API_URL, http);
  }

  isNameExists(name: string) {
    return this.http.get(Constants.MINISTRYSECTOR_API_URL + '/is-name-exists/' + name);
  }
}
