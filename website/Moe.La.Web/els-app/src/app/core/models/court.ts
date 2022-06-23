import { KeyValuePairs } from "./key-value-pairs";

export interface Court extends KeyValuePairs {
  litigationType: string;
  courtCategory: string;
}

export interface SaveCourt extends KeyValuePairs {
  courtTypeId: number;
}


