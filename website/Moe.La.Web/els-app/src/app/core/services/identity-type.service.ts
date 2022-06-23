import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IdentityTypeService extends DataService {

  constructor(http: HttpClient) {
    super(environment.API_URL + 'identity-types', http);
  }

  isNameExists(name: string) {
    return this.http.get(environment.API_URL + 'identity-types/is-name-exists/' + name);
  }
}
