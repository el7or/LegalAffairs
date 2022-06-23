import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Constants } from '../constants';
import { MoamalaChangeStatus } from '../models/moamalat';

@Injectable({ providedIn: 'root' })
export class MoamalatService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.MOAMALAT_API_URL, http);
  }

  changeStatus(moamalaChangeStatus: MoamalaChangeStatus) {
    return this.http.put(`${Constants.MOAMALAT_API_URL}/change-status`, moamalaChangeStatus);

  }

  return(id, note) {
    return this.http.put(`${Constants.MOAMALAT_API_URL}/return/${id}?note=${note}`, {});

  }

  updateWorkItemType(moamalaId: number, workItemTypeId: number,subWorkItemTypeId:number) {
    return this.http.put(`${Constants.MOAMALAT_API_URL}/update-work-item-type`, {
      id: moamalaId,
      workItemTypeId: workItemTypeId,
      subWorkItemTypeId:subWorkItemTypeId
    });
  }

  updateRelatedId(moamalaId: number, relatedId: number) {
    return this.http.put(`${Constants.MOAMALAT_API_URL}/update-related-id`, {
      id: moamalaId,
      relatedId: relatedId
    });
  }

  printMoamalaDetails(moamalaDetails: any) {
    return this.http.post(`${Constants.MOAMALAT_API_URL}/print-moamala-details`, moamalaDetails, {
      responseType: 'blob' as 'json',
    });
  }

  getConfidentialDegrees() {
    return this.http.get(Constants.MOAMALAT_API_URL + '/confidential-degrees');
  }

  getPassTypes() {
    return this.http.get(Constants.MOAMALAT_API_URL + '/pass-types');
  }
}
