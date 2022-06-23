import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { LegalBoardStatus } from '../models/legal-board-status';
import { LegalBoardMemberQueryObject, QueryObject } from '../models/query-objects';
import { query } from 'chartist';

@Injectable({ providedIn: 'root' })
export class LegalBoardService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.LEGALBOARD_API_URL, http);
  }

  changeStatus(id: number, isActive: number) {
    return this.http.post(Constants.LEGALBOARD_API_URL + '/changeStatus/' + id + '?' + 'isActive=' + isActive, {});
  }
  getLegalBoardType() {
    return this.http.get(Constants.LEGALBOARD_API_URL + '/legal-board-type');
  }
  getLegalBoardMemberType() {
    return this.http.get(Constants.LEGALBOARD_API_URL + '/legal-board-member-type');
  }
  getLegalBoardMemberHistory(queryObject: LegalBoardMemberQueryObject) {
    return this.http.get(Constants.LEGALBOARD_API_URL + `/legal-board-member-history?` + this.toQueryString(queryObject));
  }
  changeLegalBoardMemberActive(LegalBoardMemberId: number, IsActive: boolean) {
    return this.http.put(Constants.LEGALBOARD_API_URL + '/change-legal-board-member-active?LegalBoardMemberId=' + LegalBoardMemberId + '&&IsActive=' + IsActive, {});
  }
  // assignMemoToBoard(legalBoardMemo: any) {
  //   return this.http.post(Constants.LEGALBOARD_API_URL + `/assign-memo-to-board`, legalBoardMemo);
  // }

  createLegalBoardMemo(legalBoardMemo: any) {
    return this.http.post(Constants.LEGALBOARD_API_URL + `/legal-board-memo`, legalBoardMemo);
  }

  updateLegalBoardMemo(legalBoardMemo: any) {
    return this.http.put(Constants.LEGALBOARD_API_URL + `/legal-board-memo`, legalBoardMemo);
  }

  getLegalBoard() {
    return this.http.get(Constants.LEGALBOARD_API_URL + `/legal-boards`);
  }

  getLegalBoardMembers(boardId: number) {
    return this.http.get(Constants.LEGALBOARD_API_URL + '/legal-board-members?boardId=' + boardId);
  }

  // meeting api
  getMeetingsWithQuery(queryObject: any) {
    return this.http.get(this.url + '/board-meeting' + '?' + this.toQueryString(queryObject));
  }

  getMeeting(id: number | string) {
    return this.http.get(this.url + '/board-meeting/' + id);
  }

  getMeetingByBoardAndMemo(boardId:number , memoId: number) {
    return this.http.get(this.url + `/board-meeting/${boardId}/${memoId}`);
  }

  createMeeting(resource: any) {
    return this.http.post(this.url + '/board-meeting', resource)
  }

  updateMeeting(resource: any) {
    return this.http.put(this.url + '/board-meeting', resource)
  }

}
