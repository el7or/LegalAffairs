import { KeyValuePairs } from "./key-value-pairs";

interface BasicModel extends KeyValuePairs {
  updateUserFullName: string;
  updateDate: string;
  isDeleted: boolean;
}
