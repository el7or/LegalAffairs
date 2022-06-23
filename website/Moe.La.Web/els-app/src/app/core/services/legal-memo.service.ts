import { DataService } from './data.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { QueryObject } from '../models/query-objects';
import { LegalMemoList } from 'app/core/models/legal-memo';

@Injectable({
  providedIn: 'root',
})
export class LegalMemoService extends DataService {
  memos: LegalMemoList[] = [];

  constructor(http: HttpClient) {
    super(Constants.LEGALMEMO_API_URL, http);
  }
  getLegalMemoStatus() {
    return this.http.get(Constants.LEGALMEMO_API_URL + '/legal-memo-status');
  }
  getLegalMemoTypes() {
    return this.http.get(Constants.LEGALMEMO_API_URL + '/legal-memo-types');
  }
  changeLegalMemoStatus(legalMemoId: number, legalMemoStatusId: number) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/change-legal-memo-status?legalMemoId=${legalMemoId}&legalMemoStatusId=${legalMemoStatusId}`,
      null
    );
  }
  raisToAllBoardsHead(id: number) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/rais-to-boardshead?id=${id}`,
      null
    );
  }

  approve(id: number) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/approve?id=${id}`,
      null
    );
  }

  rejectFrom(id: number, note) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/reject?id=${id}&note=${note}`,
      null
    );
  } 
  reject(id: number, note) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/reject/${id}?note=${note}`,
      null
    );
  }
  readLegalMemo(legalMemoId: number) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/read-legal-memo?legalMemoId=${legalMemoId}`,
      null
    );
  }

  deleteMemo(deletionReason: any) {
    return this.http.put(Constants.LEGALMEMO_API_URL + '/delete-legal-memo', deletionReason);
  }

  getLegalMemosWithQuery(queryObject: QueryObject) {
    return this.http.get(
      Constants.LEGALMEMO_API_URL +
      '/accepted-memos/?' +
      this.toQueryString(queryObject)
    );
  }
  getAllByCaseId(caseId: number) {
    return this.http.get(Constants.LEGALMEMO_API_URL + '/GetAllByCaseID/?caseId=' + caseId);
  }
  ////notes
  getNotesWithQuery(queryObject: QueryObject) {
    return this.http.get(
      Constants.LEGALMEMO_API_URL +
      '/notes/' +
      '?' +
      this.toQueryString(queryObject)
    );
  }
  getNote(id: number) {
    return this.http.get(Constants.LEGALMEMO_API_URL + '/note/' + id);
  }
  createNote(resource: any) {
    return this.http.post(
      `${Constants.LEGALMEMO_API_URL}/create-note`,
      resource
    );
  }
  updateNote(resource: any) {
    return this.http.put(
      `${Constants.LEGALMEMO_API_URL}/update-note`,
      resource
    );
  }
  deleteNote(id: number) {
    return this.http.delete(Constants.LEGALMEMO_API_URL + '/note/' + id);
  }
  printLegalMemo(memoDetails: any) {
    return this.http.post(Constants.LEGALMEMO_API_URL + '/print', memoDetails, {
      responseType: 'blob' as 'json',
    });
  }
  printHearingLegalMemo(hearingMemoDetails: any) {
    return this.http.post(Constants.LEGALMEMO_API_URL + '/print-hearing-memo', hearingMemoDetails, {
      responseType: 'blob' as 'json',
    });
  }

}
