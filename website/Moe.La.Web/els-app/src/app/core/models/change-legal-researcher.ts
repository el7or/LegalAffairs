import { Guid } from 'guid-typescript';


export interface ChangeLegalResearcher{

  caseId:number;
  caseSubject:string;
  researcherName:string;
  requestDate:string;
  reason:string;
  createdBy:string;
}
