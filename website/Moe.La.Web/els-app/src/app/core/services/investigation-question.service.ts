
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';;
import { environment } from '../../../environments/environment';
import { InvestigationQuestionStatuses } from '../enums/InvestigationQuestionStatuses';
import { Constants } from '../constants';


@Injectable({ providedIn: 'root' })

export class InvestigationQuestionService extends DataService{

  constructor(http: HttpClient) {
    super(environment.API_URL + 'investigation-questions', http);
  }

  changeStatus(id: number, status: InvestigationQuestionStatuses) {
    return this.http.put(`${Constants.Investigation_Question_API_URL}/change-status`, {
      id: id,
      status: status,
    });
  }
  
  GetQuestionsStatuses() {
    return this.http.get(`${Constants.Investigation_Question_API_URL}/question-statuses`);
  } 

  isNameExists(investigationQuestion: string) {
    return this.http.post(Constants.Investigation_Question_API_URL+ '/is-name-exists', investigationQuestion);
  }
}

