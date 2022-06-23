import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root',
})
export class UserService extends DataService {
  constructor(http: HttpClient) {
    super(Constants.USER_API_URL, http);
  }

  getUserDetails(userId: string) {
    return this.http.get(Constants.USER_API_URL + '/' + userId);
  }

  getConsultants(name: string, legalBoardId: number) {
    return this.http.get(Constants.USER_API_URL + `/consultants?name=${name}&&legalBoardId=${legalBoardId}`);
  } 

  deletePhoto(id) {
    return this.http.delete(Constants.USER_API_URL +'/delete-photo/' + id);
  } 

  getRoles(userId: string) {
    return this.http.get(Constants.USER_API_URL + '/roles/' + userId);
  }

  updateEnabledUser(userId: string, enable: boolean) {
    return this.http.put(
      Constants.USER_API_URL + '/' + userId + '/enabled/' + enable, null
    );
  }

  // createHashCode(email: string) {
  //   return this.http.get(
  //     environment.API_URL + 'users/create-hash-code/' + email
  //   );
  // }

  // async isUserExists(email: string): Promise<boolean> {
  //   let isUserExists: boolean = false;

  //   await this.http.get(environment.API_URL + 'users/is-user-exists/' + email);

  //   return isUserExists;
  // }



  // deletePhoto(id: number) {
  //   return this.http.delete(environment.API_URL + 'users/delete-photo/' + id);
  // }

  // async isSuperUser(userId: number): Promise<boolean> {
  //   //let isSuperUser: boolean;

  //   return (await this.http.get(
  //     environment.API_URL + 'users/is-super-user/' + userId
  //   )) as any;

  //   //return isSuperUser;
  // }

  // changePassword(resource: string) {
  //   return this.http.put(
  //     environment.API_URL + 'users/change-password',
  //     resource
  //   );
  // }

  // resetPassword(resource: string) {
  //   return this.http.put(
  //     environment.API_URL + 'users/reset-password',
  //     resource
  //   );
  // }

  // updateMobile(resource: any) {
  //   return this.http.put(
  //     environment.API_URL + 'users/update-mobile/' + resource.id,
  //     resource
  //   );
  // }

  //getArRole(enName) {
  //  if (this.rolesTranslator.find(t => t.en == enName))
  //    return this.rolesTranslator.find(t => t.en == enName).ar;
  //  else
  //    return "-------";
  //}

  //rolesTranslator = [
  //  { en: "admin", ar: "" },
  //  { en: "president", ar: "" },
  //  { en: "vicePresident", ar: "" },
  //  { en: "headOfJudiciary", ar: "" },
  //  { en: "administratorOfJudiciary", ar: "" },
  //  { en: "BranchManager", ar: "" },
  //  { en: "consultant", ar: "" },
  //  { en: "legalAssistant", ar: "" },
  //  { en: "filesProvider", ar: "" },
  //  { en: "administratorOfContracts", ar: "مدير العقود" }
  //]
}
