import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class MoamalatTransactionService extends DataService {

    constructor(http: HttpClient) {
        super(Constants.MOAMALATTRANSACTION_API_URL, http);
    }
}
