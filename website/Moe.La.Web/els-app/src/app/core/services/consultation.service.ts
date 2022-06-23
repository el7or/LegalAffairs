import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class ConsultationService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.CONSULTATION_API_URL , http);
  }
  getConsultationStatus() {
    return this.http.get(Constants.CONSULTATION_API_URL + '/consultation-status');
  }

  consultationReview(review: any) {
    return this.http.post(Constants.CONSULTATION_API_URL + '/consultation-review', review);
  }

  deleteVisual(id: any) {
    return this.http.delete(Constants.CONSULTATION_API_URL + '/delete-visual/' + id);
  }

}