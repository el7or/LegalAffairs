import { CaseListItem } from './case';
import { SimpleUser } from './user';


export interface CaseChangeConsultantRequest {
  id: number;
  case: CaseListItem;
  addUser: SimpleUser;
  proposedConsultant: SimpleUser;
  requestReason: string;
  requestDate: string;
  supervisor: SimpleUser;
  isAccept?: boolean;
}

export interface SaveCaseChangeConsultantRequest {
  id: number;
  caseId: number;
  addUserId: string;
  proposedConsultantId: string;
  requestReason: string;
  supervisorId: string;
  isAccept?: boolean;
}
