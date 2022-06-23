import { RequestDto } from "./request";

export class ExportCaseJudgmentRequest {
  id: number = 0;
  caseId: number = 0;
  replyNote: string = '';

  request!: RequestDto;

}
