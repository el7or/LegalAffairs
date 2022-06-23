import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

import { Constants } from '../constants';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class AttacmentService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.ATTACHMENT_CONSULTANT_API_URL, http);
  }

  downloadFile(id: string, name: string) {
    return this.http.get(`${Constants.ATTACHMENT_CONSULTANT_API_URL}/download/${id}/${name}`,
      {
        responseType: 'blob' as 'json',
      }
    );
  }

  uploadAttachment(
    file:File , type = null
  ) {
    var formData = new FormData();
    formData.append("file", file);
    formData.append("attachmentTypeId", type);

    return this.http
      .post(
        environment.API_URL +`attachments`,
        formData
      );
  }
}
