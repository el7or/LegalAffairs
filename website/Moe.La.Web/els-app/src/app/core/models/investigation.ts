import { BaseModel } from './base-model';

export class InvestigationListItem extends BaseModel<number> {
  investigationNumber!: number;
  startDate!: string;
  startDateHigri!: string;
  startTime!:string;
  subject!: string;
  investigatorFullName!: string;
  investigationStatus!: string;
  isHasCriminalSide!: boolean;
}
