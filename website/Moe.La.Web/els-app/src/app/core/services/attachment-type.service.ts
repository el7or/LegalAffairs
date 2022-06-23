import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { AttachmentType } from '../models/attachment-type';

@Injectable({ providedIn: 'root' })
export class AttachmentTypeService extends DataService {

  constructor(http: HttpClient) {
    super(Constants.ATTACHMENTTYPE_API_URL, http);
  }

  isNameExists(attachmentType: AttachmentType) {
    return this.http.post(Constants.ATTACHMENTTYPE_API_URL + '/is-name-exists', attachmentType);
  }

  getGroupName() {
    return this.http.get(Constants.ATTACHMENTTYPE_API_URL + '/group-names');
  }
}
