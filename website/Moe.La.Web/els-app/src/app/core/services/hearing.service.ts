import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Constants } from '../constants';
import {
  HearingQueryObject,
  HearingUpdateQueryObject,
  UserQueryObject,
} from '../models/query-objects';

@Injectable({ providedIn: 'root' })
export class HearingService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.HEARINGS_API_URL, http);
  }

  getHearingStatus() {
    return this.http.get(Constants.HEARINGS_API_URL + '/hearing-status');
  }

  getHearingType() {
    return this.http.get(Constants.HEARINGS_API_URL + '/hearing-type');
  }

  receivingJudgment(resource: any) {
    return this.http.put(
      `${Constants.HEARINGS_API_URL}/receiving-judgment`,
      resource
    );
  }

  // closeAndCreate(resource: any) {
  //   return this.http.post(
  //     Constants.HEARINGS_API_URL + '/close-and-create',
  //     resource
  //   );
  // }

  getMaxHearingNumber(caseId: number) {
    return this.http.get(
      Constants.HEARINGS_API_URL + '/max-hearing-number/' + caseId
    );
  }

  getFirstHearingId(caseId: number) {
    return this.http.get(
      Constants.HEARINGS_API_URL + '/first-hearing-id/' + caseId
    );
  }

  isHearingNumberExists(resource: any) {
    return this.http.put(
      `${Constants.HEARINGS_API_URL}/hearing-number-exists`,
      resource
    );
  }

  onPrint(queryObject: HearingQueryObject) {
    return this.http.get(
      Constants.HEARINGS_API_URL +
      '/make-pdf-report?' +
      this.toQueryString(queryObject),
      {
        responseType: 'blob' as 'json',
      }
    );
  }

  assignHearingTo(hearingId: number, attendantId: string) {
    return this.http.get(
      Constants.HEARINGS_API_URL + '/assign-to/' + hearingId + '/' + attendantId
    );
  }

  getAllHearingUpdates(queryObject: HearingUpdateQueryObject) {
    return this.http.get(
      Constants.HEARINGS_API_URL +
      '/hearing-updates?' +
      this.toQueryString(queryObject)
    );
  }

  getHearingUpdate(hearingId: number) {
    return this.http.get(
      Constants.HEARINGS_API_URL + '/hearing-updates/' + hearingId
    );
  }

  addHearingUpdate(hearingUpdate: any) {
    return this.http.post(
      Constants.HEARINGS_API_URL + '/add-hearing-update',
      hearingUpdate
    );
  }

  editHearingUpdate(hearingUpdate: any) {
    return this.http.put(
      Constants.HEARINGS_API_URL + '/edit-hearing-update',
      hearingUpdate
    );
  }

  getConsultantsAndResearchers(queryObject: UserQueryObject) {
    return this.http.get(
      Constants.HEARINGS_API_URL + '/consultants-and-researchers' + '?' + this.toQueryString(queryObject)
    );
  }
}
