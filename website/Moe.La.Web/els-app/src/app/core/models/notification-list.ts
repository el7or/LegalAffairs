import { KeyValuePairs } from './key-value-pairs';
export class NotificationList {
  id: number = 0;
  text: string = '';
  type: KeyValuePairs= { id:0, name: '' };
  createdOn: string='';
  creationTime:string='';
  isRead?: boolean=false;
}

