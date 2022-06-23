import { Pipe, PipeTransform } from "@angular/core";
import { AuthService } from "../../core/services/auth.service";

@Pipe({
  name: "caseButtonName",
})
export class CaseButtonNamePipe implements PipeTransform {
  constructor(private authService: AuthService) {}

  transform(stageName: string, buttonName: string): string {
    if (buttonName == "Rating") {
      if (stageName == "NewCase") return "تقييم الطلب";
      else if (stageName == "Rated") return "تعديل التقييم";
      return stageName;
    } else if (buttonName == "AssignConsultant") {
      if (stageName == "Accepted") return "تكليف مستشار";
      else if (
        stageName == "ConsultantAssigned" ||
        stageName == "InfoCompleted" ||
        stageName == "HearingStarted" ||
        stageName == "HearingClosed"
      )
        return "تغيير المستشار المكلف";
      return stageName;
    } else if (buttonName == "Edit") {
     /* if ( stageName == "ConsultantAssigned"  )
        return "استكمال بيانات القضية";*/
      if (
        stageName == "NewCase" ||
        stageName == "Rated" ||
        stageName == "Accepted" ||
        stageName == "ConsultantAssigned" ||
        stageName == "InfoCompleted" ||
        stageName == "HearingStarted" ||
        stageName == "HearingClosed" ||
        stageName == "ConsultantAssigned" ||
        stageName == "CaseJudgement"
      )
        return "تعديل بيانات القضية";
      return stageName;
    } else if (buttonName == "Judgement") {
      if (stageName == "CaseJudgement") return "تعديل الحكم الصادر";
      return "إضافة الحكم الصادر";
    } else return stageName;
  }
}
