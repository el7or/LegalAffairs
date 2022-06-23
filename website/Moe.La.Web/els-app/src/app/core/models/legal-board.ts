import { KeyValuePairs } from "./key-value-pairs";

export interface LegalBoardListItem extends KeyValuePairs {
  status: KeyValuePairs,
  type: string,
  legalBoardMembers: LegalBoardMembersListItem[],
  createdBy: string
}

export interface LegalBoard extends KeyValuePairs {

}

export interface LegalBoardMembers {
  id: number,
  userId: string,
  membershipType: number,
  startDate: string,
  isActive: boolean,
  isSelected?: boolean
}

export interface LegalBoardMembersListItem {
  id: number,
  userName: string,
  userId: string,
  membershipType: KeyValuePairs,
  startDate: string,
  startDateHigri?:string,
  isActive: boolean,
  isSelected?: boolean
}


