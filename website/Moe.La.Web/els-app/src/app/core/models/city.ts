import { KeyValuePairs } from "./key-value-pairs";
import { Province } from "./province";

export interface City extends KeyValuePairs {
  province: string;
  provinceId: number;
}

export interface SaveCity extends KeyValuePairs {
  provinceId: number;
}
