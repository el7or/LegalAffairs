import { Attachment } from './attachment';
import { KeyValuePairs } from './key-value-pairs';

export interface InvestigationRecord {
}

export class SaveInvestigationRecord {
    id: number = 0;

    investigatorId!: string;

    startDate!: Date;

    endDate!: Date;

    visuals: string = '';

    recordNumber: string = '';

    recordStatus!: number;

    investigationId!: number;

    attachments?: Attachment[];

    investigationRecordParties?: InvestigationRecordParty[];

    investigationRecordQuestions?: InvestigationRecordQuestion[];

    investigationRecordInvestigators: InvestigationRecordInvestigators[] = [];

    attendants: InvestigationRecordAttendant[] = [];
}
export class InvestigationRecordDetails {
    id: number = 0;

    investigatorId!: string;

    startDate!: Date;

    endDate!: Date;

    visuals: string = '';

    recordNumber: string = '';

    recordStatus!: number;

    investigationId!: number;

    isRemote: boolean = false;

    attachments: Attachment[] = [];

    investigationRecordParties: InvestigationRecordParty[] = [];

    investigationRecordQuestions: InvestigationRecordQuestion[] = [];

    investigationRecordInvestigators: InvestigationRecordInvestigators[] = [];

    attendants: InvestigationRecordAttendant[] = [];

}


export class InvestigationRecordParty {

    id?: number;

    partyName: string = '';

    identityNumber: string = '';

    birthDate!: Date;

    investigationRecordPartyTypeId: number = 0;

    InvestigationRecordPartyTypeName: string = '';

    staffType?: number = 0;

    staffTypeName: string = '';

    assignedWork: string = '';

    workLocation: string = '';

    lastQualificationAttained: string = '';

    appointmentStatus?: number = 0;

    appointmentStatusName: string = '';

    commencementDate?: Date;

    identityDate?: Date;

    identitySource: string = '';

    investigationPartyPenalties: InvestigationRecordPartyPenalty[] = [];

    evaluations: Evaluation[] = [];

    educationalLevels: EducationalLevel[] = [];

    birthDateOnHijri?: string = '';
}
export class InvestigationRecordPartyDetails {
    id?: number;

    partyName: string = '';

    identityNumber: string = '';

    birthDate!: Date;

    birthDateOnHijri: string = '';

    investigationRecordPartyType: KeyValuePairs = new KeyValuePairs();

    staffType: KeyValuePairs = new KeyValuePairs();

    assignedWork: string = '';

    workLocation: string = '';

    lastQualificationAttained: string = '';

    appointmentStatus: KeyValuePairs = new KeyValuePairs();

    commencementDate?: Date;

    commencementDateOnHijri: string = '';

    identityDate?: Date;

    identityDateOnHijri: string = '';

    identitySource: string = '';

    investigationPartyPenalties: InvestigationRecordPartyPenalty[] = [];

    evaluations: Evaluation[] = [];

    educationalLevels: EducationalLevel[] = [];
}

export class InvestigationRecordPartyPenalty {
    penalty: string = '';

    reasons: string = '';

    decisionNumber = 11;

    date?: Date;

    dateOnHijri: string = '';
}

export class Evaluation {

    percentage: number = 0;

    year: number = 0;
}

export class EducationalLevel {
    educationLevel: string = '';

    class = '';

    classNumber = "";

    residenceAddress: string = '';
}

export interface InvestigationRecordQuestion {

    id?: number;

    questionId?: number;

    question: string;

    answer: string;

    assignedTo: any;
}

export interface InvestigationRecordInvestigators {
    investigatorId: string;
}

export class InvestigationRecordAttendant {
    id?: number;
    fullName: string = '';
    assignedWork: string = '';
    workLocation: string = '';
    representativeOfId!: number;
    representativeOf!: KeyValuePairs;
    details: string = '';
    identityNumber: string = '';

    public constructor(init?: Partial<InvestigationRecordAttendant>) {
        Object.assign(this, init);
    }
}
