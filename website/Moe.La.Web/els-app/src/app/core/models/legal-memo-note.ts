
export interface LegalMemoNoteList {
    id: number;
    legalMemoId: number;
    legalBoardId?: number;
    reviewNumber: number;
    text: string;
    createdOn: string;
    creationTime: string;
    createdBy: string;
    isClosed?: boolean;
}

export class LegalMemoNote {
    id?: number = 0;
    legalMemoId: number = 0;
    legalBoardId?: number = 0;
    reviewNumber?: number;
    text?: string;
    isClosed?: boolean;
}
