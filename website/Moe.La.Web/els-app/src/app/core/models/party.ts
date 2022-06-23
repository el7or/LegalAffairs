export class Party {
  id : number;
  name : string;
  companyName: string;
  partyTypeName: string;
  partyType: number;
  organizationName: string;
}

export class PartyDetails {
  id : number;
  name : string;
  companyName: string;
  partyTypeName: string;
  organizationName:string;
  partyType:PartyType;
}


export class PartyType
{
  id: number;
  name: string;
}

export class PartyStatus
{
  data : any[] = [
    {
      id: 1,
      name: 'بالاصالة عن نفسه',
    },
    {
      id: 2,
      name: 'وكيل',
    },
    {
      id: 3,
      name: 'ممثل جهة',
    }
  ];
}
