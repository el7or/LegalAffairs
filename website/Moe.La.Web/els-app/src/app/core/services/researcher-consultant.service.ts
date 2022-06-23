import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { Constants } from '../constants';
import { environment } from '../../../environments/environment';
 
@Injectable({
  providedIn: 'root',
})
export class ResearcherConsultantService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.RESEARCHER_CONSULTANT_API_URL, http);
  }   
    
  assignResearcherConsultant( researcherId:string , consultantId:string) {
    return this.http.post(
      Constants.RESEARCHER_CONSULTANT_API_URL + '/researcher-consultant/',
      { researcherId, consultantId}
    );
  }
 
}
