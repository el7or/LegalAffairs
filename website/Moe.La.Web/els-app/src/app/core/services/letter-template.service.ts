import { Constants } from '../constants';
import { DataService } from './data.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LetterTemplateService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.LETTER_TEMPLATE_API_URL, http);
  }


}
