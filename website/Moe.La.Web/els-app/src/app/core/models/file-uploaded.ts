import { KeyValuePairs } from './key-value-pairs';
export interface Image {
  id: number;
  fileName: string;
}

export interface FileUploaded {
  id: number;
  fileTitle: string;
  filePath: string;
  fileSize: number;
  uploadDate: Date;
  senderId: number;
  senderType: number;
  parentId?: number;
  parentType?: number;
  rootId?: number;
  rootType?: number;
}

export interface SaveFileUploaded {
  id: number;
  fileTitle: string;
  fileTypeId?: number;
}

export enum SenderType {
  case = 1,
  hearing = 2,
  procedure = 3,
  procedureReply = 4,
  requestFile = 5,
  hearingClosingReport = 6,
  hearingProcedure = 8,
  hearingRequestFile = 9,
  caseJudgement = 10,
  customer = 11,
  adversary = 12,
  investigation = 13,
  investigationRequestFile = 14,
  investigationConsultantCompleted = 15,
  investigationGMCompletedApproved = 16,
  investigationRecord = 17,
  agency = 18,
  consultation = 19,
  consultationRequestFile = 20,
  consultationLegalOpinion = 21
}
