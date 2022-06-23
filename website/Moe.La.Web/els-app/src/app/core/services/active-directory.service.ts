import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { DataService } from './data.service';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class MimService extends DataService {

    constructor(http: HttpClient) {
        super(Constants.AD_API_URL, http);
    }

    getIdentity(username: string) {
        return this.http.get(Constants.AD_API_URL + `/${username}`);
    }
}
