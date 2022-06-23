import { KeyValuePairs } from './key-value-pairs';
export class LegalMemoList {
  id: number = 0;
  name: string = '';
  status: KeyValuePairs= { id:0, name: '' };
  secondSubCategory: string;
  createdByUser: KeyValuePairs<string> = { id: '', name: '' };
  createdOn: string='';
  createdOnHigri?:string;
  updatedOn: string='';
  creationTime:string='';
  updateTime:string='';
  isRead?: boolean=false;
  isDeleted?: boolean = false;

}

export class LegalMemoDetails {
  id: number = 0;
  name: string = '';
  status: KeyValuePairs = { id: 0, name: '' };
  updatedOn?: Date;
  reviewNumber?: number;
  createdByUser: KeyValuePairs<string>[] = [];
  text: string = '';
  isRead?: boolean;
  initialCaseId: number = 0;
  createdOnHigri?:string;
}

export class DeletionLegalMemo {
  id: number=0;
  deletionReason: string="";
}

export class SaveLegalMemo { }
