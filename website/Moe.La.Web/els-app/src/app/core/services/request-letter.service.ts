import { Constants } from '../constants';
import { DataService } from './data.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RequestLetterService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.REQUEST_LETTER_API_URL, http);
  }

  getReplaceDocumentRequestContent(requestId: number, letterId: number) {
    return this.http.get(Constants.REQUEST_LETTER_API_URL + `/replace-document-request-content/${requestId}/${letterId}`);
  }
  getReplaceCaseCloseRequestContent(caseId: number, letterId: number) {
    return this.http.get(Constants.REQUEST_LETTER_API_URL + `/replace-case-close-request-content/${caseId}/${letterId}`);
  }

  replaceDeplartment(contnet, departmentName) {
    return this.http.post(Constants.REQUEST_LETTER_API_URL + `/replace-department`, { contnet, departmentName });
  }
  getByRequestId(requestId: number) {
    return this.http.get(Constants.REQUEST_LETTER_API_URL + `/get-by-request/?id=${requestId}`);
  }
}
