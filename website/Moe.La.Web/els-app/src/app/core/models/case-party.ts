import { Party } from './party';

export class CaseParty {
    id?: number;
    name: string = '';
}

export class CasePartyDetails{
  id: number;
  partyId: number;
  caseId : number;
  partyStatus: number;
  partyStatusName: string;
  party: Party;
  organizationName: string;
}

export class CasePartyModel {
  id: number;
  partyId: number;
  caseId : number;
  partyStatus: number;
}
