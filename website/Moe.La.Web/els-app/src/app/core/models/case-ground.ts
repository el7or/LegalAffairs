export class CaseGround {
  id:number=0;
  caseId: number=0;
  text: string='';

  public constructor(init?: Partial<CaseGround>) {
    Object.assign(this, init);
  }
}
