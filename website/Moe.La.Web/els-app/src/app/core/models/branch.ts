import { KeyValuePairs } from "./key-value-pairs";

export interface Branch extends KeyValuePairs {
  sector: KeyValuePairs;
  parentId: number;
  departments: number[];
}

export interface SaveBranch extends KeyValuePairs {
  sectorId: number;
}

