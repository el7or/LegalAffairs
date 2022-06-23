export class BoardMeetingListItem {
  id:number;
  meetingPlace: string;
  meetingDate: string;
  meetingTime: string;
  board: string;
  memo: string;
}

export class BoardMeeting {
  boardMeetingId: number= 0;
  legalMemoId: number = 0;
  legalBoardId: number = 0;
}
