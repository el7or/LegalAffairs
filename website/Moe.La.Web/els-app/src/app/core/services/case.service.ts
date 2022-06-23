import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { EnumValue } from '../models/enum-value';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { Guid } from 'guid-typescript';
import { CaseStatus } from '../enums/CaseStatus';
import { KeyValuePairs } from '../models/key-value-pairs';
import { CaseQueryObject } from '../models/query-objects';
import { CaseListItem } from '../models/case';

@Injectable({
  providedIn: 'root',
})
export class CaseService extends DataService {
  stages: EnumValue[] = [];
  courtTypes: EnumValue[] = [];
  judgementResults: EnumValue[] = [];

  private cases: CaseListItem[] = [];

  constructor(http: HttpClient) {
    super(Constants.CASE_API_URL, http);
  }

  getCaseStatus() {
    return this.http.get(Constants.CASE_API_URL + '/case-status');
  }

  getAllCases() {
    return this.http.get(Constants.CASE_API_URL + '/full-data');
  }
  getCaseSources() {
    return this.http.get(Constants.CASE_API_URL + '/case-sources');
  }

  getLitigationTypes() {
    return this.http.get(Constants.CASE_API_URL + '/litigation-types');
  }

  getPartyTypes() {
    return this.http.get(Constants.CASE_API_URL + '/party-types');
  }

  getMinistryLegalStatus() {
    return this.http.get(Constants.CASE_API_URL + '/ministry-legal-status');
  }

  getJudgementResults() {
    return this.http.get(Constants.CASE_API_URL + '/judgement-results');
  } 

  changeStatus(id: number, status: CaseStatus, note?: string) {
    return this.http.put(`${Constants.CASE_API_URL}/change-status`, {
      id: id,
      status: status,
      note: note
    });
  }
  sendToBranchManager(id: number, note: string, branchId: number) {
    return this.http.put(`${Constants.CASE_API_URL}/send-to-general-manager`, {
      id: id,
      note,
      branchId,
    });
  }

  assignToResearcher(caseId: number, researcherId: Guid, note?: string) {
    return this.http.put(`${Constants.CASE_API_URL}/assign-researcher`, {
      caseId: caseId,
      researcherId: researcherId,
      note: note,
    });
  }

  addCaseRule(caseRule: any) {
    return this.http.post(Constants.CASE_API_URL + '/add-rule', caseRule);
  }

  printCaseDetails(caseDetails: any) {
    return this.http.post(`${Constants.CASE_API_URL}/print-case-details`, caseDetails, {
      responseType: 'blob' as 'json',
    });
  }

  printCaseList(caseQueryObject: CaseQueryObject) {
    return this.http.post(`${Constants.CASE_API_URL}/print-case-list`, caseQueryObject, {
      responseType: 'blob' as 'json',
    });
  }

  exportExcelCaseList(caseQueryObject: CaseQueryObject) {
    return this.http.post(`${Constants.CASE_API_URL}/excel-case-list`, caseQueryObject, {
      responseType: 'blob' as 'json',
    });
  }
  receivingJudgmentInstrument(resource: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/receive-judgment-instrument`,
      resource
    );
  }
  getreceivingJudgmentInstrument(Id: any) {
    return this.http.get(
      `${Constants.CASE_API_URL}/receive-judgment-instrument?Id=${Id}`
    );
  }
  editReceivingJudgmentInstrument(resource: any) {
    return this.http.put(
      `${Constants.CASE_API_URL}/receive-judgment-instrument`,
      resource
    );
  }

  createInitialCase(initialCase: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/Create-Case`,
      initialCase
    );
  }
  createObjectionCase(objectionCase: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/Create-objection-Case`,
      objectionCase
    );
  }

  getCaseParties(Id: number) {
    return this.http.get(Constants.CASE_API_URL + '/parties/' + Id);
  }

  addCaseParty(caseParty: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/add-case-party`,
      caseParty
    );
  }

  updateCaseParty(caseParty: any) {
    return this.http.put(
      `${Constants.CASE_API_URL}/update-case-party`,
      caseParty
    );
  }

  deleteCaseParty(Id: any) {
    return this.http.delete(
      `${Constants.CASE_API_URL}/delete-case-party/${Id}`
    );
  }

  //case grounds
  addGround(caseGround: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/ground`,
      caseGround
    );
  }
  editGround(caseGround: any) {
    return this.http.put(
      `${Constants.CASE_API_URL}/ground`,
      caseGround
    );
  }

  deleteGround(id: any) {
    return this.http.delete(
      `${Constants.CASE_API_URL}/ground/${id}`
    );
  }

  getCaseGrounds(id) {
    return this.http.get(
      `${Constants.CASE_API_URL}/grounds/${id}`);
  }

  updateCaseGrounds(caseGrounds: any) {
    return this.http.put(
      `${Constants.CASE_API_URL}/grounds`,
      caseGrounds
    );
  }

  // case moamalat
  addCaseMoamalat(caseMoamalat: any) {
    return this.http.post(
      `${Constants.CASE_API_URL}/moamalat`,
      caseMoamalat
    );
  }

  deleteCaseMoamalat(caseId: any, moamalaId: any) {
    return this.http.delete(
      `${Constants.CASE_API_URL}/${caseId}/moamalat/${moamalaId}`
    );
  }

  getCaseMoamalat(id) {
    return this.http.get(
      `${Constants.CASE_API_URL}/moamalat/${id}`);
  }

  /////////
  updateAttachments(attachments: any) {
    return this.http.put(
      `${Constants.CASE_API_URL}/attachments`,
      attachments
    );
  }


  getPartyStatuses() {
    return this.http.get(
      `${Constants.CASE_API_URL}/parties-status`);
  }

  addNext(data: any) {
    return this.http.post(Constants.CASE_API_URL + '/next', data);
  }
}
