import { RequestStatus } from '../enums/RequestStatus';
import { RequestTypes } from '../enums/RequestTypes';
import { KeyValuePairs } from './key-value-pairs';


export class CaseSupportingDocumentRequest {
  id: number = 0;
  hearingId?: number;
  parentId?: number;
  request: Request = new Request();
  consigneeDepartmentId?: number;
  consigneeDepartment: KeyValuePairs;
  replyNote: string = '';
  documents: CaseSupportingDocumentRequestItem[] = [];
  caseId: number;
  public constructor(init?: Partial<CaseSupportingDocumentRequest>) {
    Object.assign(this, init);
  }
}

export class CaseSupportingDocumentRequestItem {
  name: string = '';
  documentRequestId?: number;
  public constructor(init?: Partial<CaseSupportingDocumentRequestItem>) {
    Object.assign(this, init);
  }
}

export class Request {
  id?: number;
  requestType: RequestTypes;
  requestStatus: RequestStatus = RequestStatus.Draft;
  caseId: number = 0;
  note: string = '';
  letter: RequestLetter;
  public constructor(init?: Partial<Request>) {
    Object.assign(this, init);
  }
}
export class RequestLetter {
  requestId?: number;
  text: string = '';
  public constructor(init?: Partial<RequestLetter>) {
    Object.assign(this, init);
  }
}


