import { KeyValuePairs } from "./key-value-pairs";

export interface Province extends KeyValuePairs {

  cities: KeyValuePairs[];
}
