
import { Guid } from 'guid-typescript';

export class Attachment {
  id?: Guid;
  name: string = '';
  size?: number;
  file?: any;
  attachmentTypeId?: number;
  attachmentType?: string;
  isPhoto?: boolean;
  createdOn?: Date;
  createdOnHigri?: string = '';
  formateSize: string = '';
  isDraft= false;
  isDeleted= false;

}

export class HearingAttachment {
  id?: string;
  name: string = '';
  size?: number;
  file: any;
  attachmentTypeId?: number;
  isPhoto?: boolean;
  createdOnHigri?: string = '';
}

export class HearingUpdateAttachment {
  id?: string;
  name: string = '';
  size?: number;
  file?: any;
  attachmentTypeId?: number;
  isPhoto?: boolean;
}

export enum GroupNames {
  Case = 1,
  Hearing = 2,
  Memo = 3,
  CaseRule = 4,
  HearingUpdate = 5,
  RepresentativeLetterImage = 6,
  InvestigationRecord = 7,
  Moamala = 8
}