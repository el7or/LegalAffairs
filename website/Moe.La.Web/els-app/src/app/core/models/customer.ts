import { KeyValuePairs } from "./key-value-pairs";
import { City } from './city';

export interface Customer extends KeyValuePairs {
  partyType: KeyValuePairs;
  identityType: KeyValuePairs;
  identityNumber: string;
  city: City;
  note: string;
  mobile: string;
  phone: string;
  fax: string;
  email: string;
  WebSite: string;
  District: string;
  Street: string;
  BuildingNo: string;
  UnitNo: string;
  ZipCode: string;
  AdditionalNo: string;
  enabled?: boolean;
  updateUserFullName?: string;
  updateDate?: string;
}


export interface SaveCustomer extends KeyValuePairs {
  partyTypeId: number;
  identityTypeId: number;
  identityNumber: string;
  cityId: number;
  note: string;
  mobile: string;
  phone: string;
  fax: string;
  email: string;
  webSite: string;
  district: string;
  street: string;
  buildingNo: string;
  unitNo: string;
  zipCode: string;
  additionalNo: string;
}
