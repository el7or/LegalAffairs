import { RequestDto } from "./request";

export class AttachedLetterRequest {
  id?: number;
  hearingId!: number;
  parentId!: number;
  request!: RequestDto;
  consigneeDepartmentId?: number;
  caseId?: number;
}