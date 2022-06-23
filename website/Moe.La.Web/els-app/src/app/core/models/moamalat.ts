import { ConfidentialDegrees } from '../enums/ConfidentialDegrees';
import { ConsultationStatus } from '../enums/ConsultationStatus';
import { MoamalaStatuses, MoamalaSteps } from '../enums/MoamalaStatuses';
import { Attachment } from './attachment';
import { BaseModel } from './base-model';
import { KeyValuePairs } from './key-value-pairs';

export class MoamalatListItem {
  id: number;

  unifiedNo!: string;
  moamalaNumber!: string;
  subject!: string;
  createdOnHigri!: string;
  createdOnTime!: string;
  passDate!: Date;
  passDateHigri!: string;
  passTime!: string;
  confidentialDegree!: KeyValuePairs;
  passType!: KeyValuePairs;
  senderDepartment!: KeyValuePairs;
  status!: KeyValuePairs;
  workItemType!: KeyValuePairs;
  subWorkItemType!: KeyValuePairs;
  description!: string;
  branch!: KeyValuePairs
  receiverDepartment!: KeyValuePairs;
  referralNote !: string;
  assignedToId?: string;
  assignedToFullName!: string;
  assigningNote !: string;
  previousStep?: MoamalaSteps;
  currentStep: MoamalaSteps;
  returningReason !: string;
  relatedId?: number;
  releatedItemsTitle?: string;
  releatedItems?: KeyValuePairs[];
  attachments?: Attachment[];
  relatedMoamalat?: MoamalatListItem[];
  consultationId?: number;
  isManual!: boolean;
  consultationStatus?: ConsultationStatus;
}

export class MoamalaDetails extends BaseModel<number> {
  unifiedNo!: string;
  moamalaNumber!: string;
  subject!: string;
  createdOnHigri!: string;
  createdOnTime!: string;
  passDate!: Date;
  passDateHigri!: string;
  passTime!: string;
  confidentialDegree!: KeyValuePairs;
  passType!: KeyValuePairs;
  senderDepartment!: KeyValuePairs;
  status!: KeyValuePairs;
  workItemType!: KeyValuePairs;
  subWorkItemType!: KeyValuePairs;
  description!: string;
  branch!: KeyValuePairs
  receiverDepartment!: KeyValuePairs;
  referralNote !: string;
  assignedToId?: string;
  assignedToFullName!: string;
  assigningNote !: string;
  previousStep?: MoamalaSteps;
  currentStep: MoamalaSteps;
  returningReason !: string;
  relatedId?: number;
  releatedItemsTitle?: string;
  releatedItems?: KeyValuePairs[];
  attachments?: Attachment[];
  relatedMoamalat?: MoamalatListItem[];
  consultationId?: number;
  isManual!: boolean;
  consultationStatus?: ConsultationStatus;
}

export class MoamalaChangeStatus {
  moamalaId?: number;
  status?: MoamalaStatuses;
  branchId?: number;
  departmentId?: number;
  assignedToId?: string;
  previousStep?: MoamalaSteps;
  currentStep?: MoamalaSteps;
  workItemTypeId?: number;
  subWorkItemTypeId?: number;
  note?: string;
  confidentialDegree?: ConfidentialDegrees
}
