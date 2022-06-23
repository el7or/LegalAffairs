import { KeyValuePairs } from "./key-value-pairs";
import { Role } from "./role";
import { Claim } from "./user-claim";

export class UserList {
  id: string = '';
  email: string = '';
  userName: string = '';
  firstName: string = '';
  lastName: string = '';
  branch: string;
  jobTitle?: string;
  enabled?: boolean;
  identityNumber?: string;
  createdOn: string = '';
  createdOnHigri: string = '';
  roleGroup: string = '';
  departmentsGroup: string = '';
  signature:string='';
  researchers?: KeyValuePairs[];
  departments?: [];
  roles: Role[] = [];
  userRoles: any = [];
}

export class UserDetails {
  id: string = '';
  consultantId: string = '';
  email?: string;
  userName: string = '';
  password?: string;
  phoneNumber?: string;
  firstName: string = '';
  secondName: string = '';
  thirdName: string = '';
  lastName: string = '';
  picture?: string;
  signature:string='';
  phoneNumberConfirmed?: boolean;
  branch: KeyValuePairs;
  jobTitle?: KeyValuePairs;
  identityNumber: string = '';
  employeeNo: string = '';
  extensionNumber?: number;
  externalType?: string;
  externalId?: string;
  token?: string;
  tokenExpired?: boolean;
  enabled?: boolean;
  roles?: Role[];
  claims?: Claim[];
  userRoleDepartments: UserRoleDepartmentDto[] = [];
}

export class ResearcherConsultant {
  consultant: string = '';
  consultantId: string = '';
  endDate: string = '';
  id: number = 0;
  researcher: string = '';
  researcherId: string = '';
  startDate: Date;
  enabled?: boolean;
  researcherDepartment: string = '';
  consultantDepartment: string = '';
  startDateHigri: string = '';
}

export class SaveUser {
  id: string = '';
  email: string = '';
  userName: string = '';
  password: string = '';
  phoneNumber: string = '';
  firstName: string = '';
  secondName: string = '';
  thirdName: string = '';
  lastName: string = '';
  branchId: number;
  jobTitleId?: number;
  identityNumber: string = '';
  extensionNumber?: number;
  token: string = '';
  tokenExpired: boolean = false;
  enabled: boolean = false;
  roles: string[] = [];
}

export class User {
  id: string = '';
  email: string = '';
  userName: string = '';
  phoneNumber: string = '';
  firstName: string = '';
  secondName: string = '';
  thirdName: string = '';
  lastName: string = '';
  branchId: number;
  jobTitleId?: number;
  extensionNumber?: number;
  enabled: boolean = false;
  identityNumber: string = '';
  employeeNo: string = '';
  roles: string[] = [];
  claims: string[] = [];
  userRoleDepartments: UserRoleDepartment[] = [];
  signature:string|null=null;
}

export class SaveUserMobile {
  id: string = '';
  phoneNumber: string = '';
  phoneNumberConfirmed: boolean = false;
}

export class Credentials {
  email: string = '';
  password: string = '';
}

export interface SimpleUser {
  id: string;
  firstName: string;
  lastName: string;
  email?: string;
  enabled?: boolean;
}

export class UserRoleDepartment {
  id?: number;
  userId: string;
  roleId: string;
  roleNameAr: string;
  departmentId: number;
}

export class UserRoleDepartmentDto {
  id?: number;
  userId: string;
  role: any;
  department: KeyValuePairs;
}

/**
 * A class holds all neccessary information for a logged-in user.
 */
export class AuthContext {
  constructor(userProfile: UserProfile, claims: SimpleClaim[]) {
    this.userProfile = userProfile;
    this.claims = claims;
  }
  userProfile: UserProfile;
  claims: SimpleClaim[];
}

/**
 * A simple class represents a claim.
 */
export class SimpleClaim {
  type: string;
  value: string;
}

/**
 * A simple class represents the user profile info.
 */
export class UserProfile {
  id: string;
  userName: string;
  fullName: string;
  branchName: string;
  roles: string[] = [];
  userRoleDepartments: AuthUserRoleDepartment[] = [];
}

/**
 * A simple class represents the relations (User, Role and Department).
 * This matches the backend definition.
 */
export class AuthUserRoleDepartment {
  id?: number;
  userId: string;
  roleId: string;
  roleName: string;
  departmentId: number;
  departmentName: string;
}

export class myUserDepartmentRole {
  role: KeyValuePairs<string>={
    id:'',
    name:''
  };
  departments: number[]=[];
}