import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;
import { DataService } from './data.service';
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.NOTIFICATION_API_URL, http);
  }

  readNotification(id: number) {
    return this.http.put(Constants.NOTIFICATION_API_URL + '/read/' + id, {})
      .catch(this.handleError);
  }
}
