/* import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { EnumValue } from "../models/EnumValue";
import { HearingProcedureQueryObject } from "../models/QueryObjects";
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient } from "@angular/common/http";

@Injectable()
export class HearingProcedureService extends DataService {
  queryObject: HearingProcedureQueryObject = {
    caseId: null,
    hearingId: null,
    typeId: null,
    addDate: null,
    supervisorId: null,
    consultantId: null,
    currentUserId: null,
    isReplied: null,
    sortBy: "addDate",
    isSortAscending: false,
    page: 1,
    pageSize: 20,
  };

  types: EnumValue[] = [];

  constructor(http: HttpClient) {
    super(environment.API_URL + "procedures", http);
  }

  getStatistics(userId?: string, period?: number) {
    return this.http
      .get(environment.API_URL + `procedures/statistics?userId=${userId}&period=${period}`)
      .map((response) => response);
  }

  async getTypes()    {
    if (!this.types.length) {
      await this.http
        .get(environment.API_URL + "procedures/types")
        .map((response) => response)

        .then( (result:any) => {
          this.types = result;
        });
    }
    return this.types;
  }

  typesTranslator = [
    { en: "Update", ar: "تحديث" },
    { en: "Task", ar: "مهمة" },
    { en: "FieldMission", ar: "مهمة ميدانية" }
  ];

  getArTypeName(enName) {
    if (this.typesTranslator.find((t) => t.en == enName))
      return this.typesTranslator.find((t) => t.en == enName).ar;
  }

  async getNextId(): Promise<number> {
    let id: number;
    await this.http
      .get(environment.API_URL + "procedures/next-id")
      .map((response) => response)

      .then( (result:any) => {
        id = result;
      });

    return id;
  }

  makePdfReport(queryObject): Observable<any> {
    return this.http.get(environment.API_URL + 'procedures/make-pdf-report' + '?' + this.toQueryString(queryObject)  )

    .catch(this.handleError);
  }
}
 */
