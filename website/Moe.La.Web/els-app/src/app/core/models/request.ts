import { SendingTypes } from './../enums/SendingTypes';
import { RequestStatus } from '../enums/RequestStatus';
import { RequestTypes } from '../enums/RequestTypes';
import { ChangeResearcherRequest } from './change-researcher-request';
import { KeyValuePairs } from './key-value-pairs';
import { RequestLetter } from './supporting-document-request';

export class RequestList {
  id:number=0; //رقم الطلب
  requestType!: KeyValuePairs; // نوع الطلب
  // نوع الطلب
  requestStatus!: KeyValuePairs; // حالة الطلب
  // حالة الطلب
  createdOn!: Date; // تاريخ تقديم الطلب
  // تاريخ تقديم الطلب
  lastTransactionDate: string=""; // تاريخ اخر حركة في الطلب
  caseId?: number; // رقم القضية
  createdBy!: KeyValuePairs; // مقدم الطلب : اسم مقدم الطلب
  // مقدم الطلب : اسم مقدم الطلب
  createdOnHigri?:string;
  lastTransactionDateHigri?:string;
}

export class Request {
  id!: number;
  caseId?: number;
  researcherId!: string;
  requestType!: string;
  note?: string;
  requestStatusId!: number;
  createdOn?: Date;
  changeResearcherRequest?: ChangeResearcherRequest;
}


export class RequestDto {
  id!: number;
  requestType!: RequestTypes;
  requestStatus!: RequestStatus;
  sendingType!: SendingTypes;
  receiverId?: string;
  receiverDepartmentId?: number;
  receiverRoleId?: string;
  relatedRequestId?: number;
  note?: string;
  createdBy?:KeyValuePairs;
  letter?: RequestLetter;

}

export class ObjectionPermitRequestDetails{

  id: number=0;
  caseId: number=0;
  request:RequestList;
  suggestedOpinon: KeyValuePairs = { id: 0, name: '' };
  note : string='';
  replyNote : string='';
}


