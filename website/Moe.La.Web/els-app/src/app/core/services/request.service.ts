import { Constants } from './../constants';
import { DataService } from './data.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TransactionQueryObject } from '../models/query-objects';

@Injectable({
  providedIn: 'root'
})
export class RequestService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.REQUEST_API_URL, http);
  }

  getRequestType() {
    return this.http.get(Constants.REQUEST_API_URL + '/request-type');
  }

  getChangeResearcherRequest(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/change-researcher-request/' + id);
  }

  getDocumentRequest(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/document-request/' + id);
  }

  getAttachedLetterRequest(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/attached-letter-request/' + id);
  }
  getDocumentRequestHistory(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/document-request-history/' + id);
  }
  changeResearcherRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/change-researcher-request', request);
  }

  getChangeResearcherToHearingRequest(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/change-hearing-researcher-request/' + id);
  }

  createChangeResearcherToHearingRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/change-hearing-researcher-request', request);
  }

  acceptChangeResearcherToHearing(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/accept-change-hearing-researcher-request', reply);
  }

  rejectChangeResearcherToHearing(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reject-change-hearing-researcher-request', reply);
  }

  createDocumentRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/document-request', request);
  }

  createAttachedLetterRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/attached-letter-request', request);
  }

  editAttachedLetterRequest(request: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/attached-letter-request', request);
  }

  addLegalMemo(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/legal-memo', request);
  }

  acceptChangeResearcher(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/accept-change-researcher-request', reply);
  }

  rejectChangeResearcher(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reject-change-researcher-request', reply);
  }

  replyDocumentRequest(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reply-document-request', reply);
  }

  editDocumentRequest(docReq: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/document-request', docReq);
  }

  printDocumentRequest(id: number) {
    return this.http.get(`${Constants.REQUEST_API_URL}/print-document-request/` + id, {
      responseType: 'blob' as 'json'
    });
  }

  printAttachedLetterRequest(id: number) {
    return this.http.get(`${Constants.REQUEST_API_URL}/print-attached-letter-request/` + id, {
      responseType: 'blob' as 'json'
    });
  }

  printExportCaseJudgmentRequest(id) {
    return this.http.post(`${Constants.REQUEST_API_URL}/print-export-case-judgment-request/${id}`, {}, {
      responseType: 'blob' as 'json'
    });
  }

  printRequest(request: any) {
    return this.http.post(`${Constants.REQUEST_API_URL}/print-request`, request, {
      responseType: 'blob' as 'json'
    });
  }

  getExportCaseJudgmentRequestHistory(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/export-case-judgment-request-history/' + id);
  }

  getHearingLegalMemoRequest(id: number) {
    return this.http.get(Constants.REQUEST_API_URL + '/hearing-legal-memo-review-request/' + id);
  }

  replyAddingMemoHearingRequest(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reply-hearing-legal-memo-review-request', reply);
  }

  GetAllTransactions(queryObject: TransactionQueryObject) {
    return this.http.get(Constants.TRANSACTION_API_URL + '?' + this.toQueryString(queryObject));
  }

  getExportCaseJudgmentRequest(id: number) {
    return this.http.get(Constants.REQUEST_API_URL + '/export-case-judgment-request/' + id);
  }

  replyExportCaseJudgmentRequest(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reply-export-case-judgment-request', reply);
  }

  expoertRequest(exportRequest: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/export-request', exportRequest);
  }

  createExportCaseJudgmentRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/export-case-judgment-request', request);
  }

  editExportCaseJudgmentRequest(request: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/export-case-judgment-request', request);
  }

  getCaseClosingReasons() {
    return this.http.get(Constants.REQUEST_API_URL + '/case-closing-reasons');
  }

  getConsultationSupportingDocument(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/consultation-request/' + id);
  }

  createConsultationSupportingDocument(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/consultation-request', request);
  }

  editConsultationSupportingDocument(request: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/consultation-request', request);
  }
  replyConsultationSupportingDocument(reply: any) {
    return this.http.put(Constants.REQUEST_API_URL + '/reply-consultation-request', reply);
  }
  //permit-request
  getCaseObjectionPermitRequest(caseId: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/case-objection-permit-request/' + caseId);
  }

  getObjectionPermitRequest(id: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/objection-permit-request/' + id);
  }

  createObjectionPermitRequest(request: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/objection-permit-request', request);
  }

  replyObjectionPermitRequest(reply: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/reply-objection-permit-request', reply);
  }

  addObjectionMemoRequest(objectionMemo: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/add-objection-legal-memo', objectionMemo);
  }

  getCaseObjectionMemoRequest(caseId: any) {
    return this.http.get(Constants.REQUEST_API_URL + '/case-objection-legal-memo-request/' + caseId);
  }

  getObjectionLegalMemoRequest(id: number) {
    return this.http.get(Constants.REQUEST_API_URL + '/objection-legal-memo-request/' + id);
  }

  replyObjectionLegalMemoRequest(reply: any) {
    return this.http.post(Constants.REQUEST_API_URL + '/reply-objection-legal-memo-request', reply);
  }

}
