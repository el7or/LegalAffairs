import { ConsultationStatus } from '../enums/ConsultationStatus';
import { KeyValuePairs } from './key-value-pairs';
import { ConsultationSupportingDocument } from './consultation-request';


export class ConsultationListItem {
  id: number = 0;
}

export class ConsultationDetails {
  id!: number;

  moamalaId!: number;

  subject!: string;

  consultationNumber!: string;

  consultationDate?: string;

  legalAnalysis!: string;

  importantElements!: string;

  isWithNote?: boolean;

  status!: KeyValuePairs;

  departmentVision: string;

  branch!: KeyValuePairs;

  department?: KeyValuePairs;

  workItemType!: KeyValuePairs;

  subWorkItemType?: KeyValuePairs;

  consultationMerits!: ConsultationMerits[];

  consultationGrounds!: ConsultationGrounds[];

  consultationVisuals!: ConsultationVisual[];

  consultationSupportingDocuments?: ConsultationSupportingDocument[];

}

export class ConsultationDto {
  id?: number;

  moamalaId!: number;

  subject!: string;

  consultationNumber?: string;

  consultationDate?: Date;

  legalAnalysis!: string;

  importantElements?: string;

  isWithNote?: boolean;

  status!: ConsultationStatus;

  departmentVision?: string;

  branchId!: number;

  departmentId!: number;

  workItemTypeId?: number;

  subWorkItemTypeId?: number;

  consultationMerits?: ConsultationMerits[];

  consultationGrounds?: ConsultationGrounds[];

  consultationVisuals?: ConsultationVisual[];
}

export class ConsultationMerits {
  text: string = '';
}

export class ConsultationGrounds {
  text: string = '';
}

export class ConsultationVisual {
  material!: string;
  visuals!: string;
}
