
export interface NotificationListItem {

  id: number;

  type :string;

  text: string;

  url: string;

  recevier: string;

  isRead: boolean;

  createdOn: Date;

  creationTime:string;

  currentDay:boolean;

  createdOnHigri:any;
}

export interface Notification {

  id: number;

  type :string;

  text: string;

  url: string;

  recevierId: string;
}
