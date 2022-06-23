import { KeyValuePairs } from './key-value-pairs';

export class CaseRuleDetails {
    id: number = 0;
    importRefNo: string = '';
    importRefDate?: Date;
    importRefDateHigri?: string;
    exportRefNo: string = '';
    exportRefDate?: Date;
    exportRefDateHigri?: string;
    judgementText: string = '';
    //litigationType: KeyValuePairs = { id: 0, name: '' };
    isFinalJudgment?: boolean;
    judgementResult: KeyValuePairs;
    judgmentBrief: string = '';
    caseRuleMinistryDepartments: KeyValuePairs<number>[] = [];
    ruleNumber: string = '';
    caseNumber: string = '';
    judgmentReasons: string = '';
    feedback: string = '';
    finalConclusions: string = '';
    attachments: any[] = [];
    caseRuleGeneralManagements?:any;
    ministrySector: string;
}
