import { KeyValuePairs } from "./key-value-pairs";

export class CaseCategory extends KeyValuePairs {

  caseSource: KeyValuePairs = { id: 0, name: '' };
  firstSubCategory:{ id: 0, name: '' ,mainCategoryId:0};
  mainCategory: KeyValuePairs = { id: 0, name: '' };
  isActive:boolean;
}

export class SaveCaseCategory extends KeyValuePairs {
  caseSourceId: number=0;
}

