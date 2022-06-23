import { AppSettingsService } from "../../core/services/app-settings.service";
import { Pipe, PipeTransform } from "@angular/core";
import { AuthService } from "../../core/services/auth.service";

@Pipe({
  name: "showButton",
})
export class CaseShowButtonPipe implements PipeTransform {

  get requiredRating(): boolean {
    return (
      this.appSettingsService.appSettings.againstCasesRequiresRating == "true"
    );
  }

  constructor(
    private authService: AuthService,
    private appSettingsService: AppSettingsService
  ) {}

  transform(
    buttonName: string,
    stageName: string,
    courtType: string,
    defendant: boolean,
    addUserId: string,
    consultantId: string
  ): boolean {
    /* ////////// Delete ///////////
    if (buttonName == "Delete") {
      if (
        this.authService.isAdmin ||
        (this.authService.currentUser?.id == addUserId && stageName == "NewCase")
      )
        return true;
    }
    ////////// Edit ///////////
    else if (buttonName == "Edit") {
      if (
        this.authService.isAdministratorOfJudiciary &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
      else if (
        this.authService.isConsultant &&
        this.authService.currentUser?.id == consultantId
      ) {
        if (
          stageName == "ConsultantAssigned" ||
          stageName == "InfoCompleted" ||
          stageName == "HearingStarted" ||
          stageName == "HearingClosed"
        )
          return true;
      } else if (courtType == "FirstInstance" && !defendant) {
        if (
          stageName == "NewCase" &&
          this.authService.currentUser?.id == addUserId
        )
          return true;
      } else {
        // defendant
        if (
          stageName == "Accepted" &&
          this.authService.currentUser?.id == addUserId
        )
          return true;
      }
    }

    //////// Rating ////////
    else if (buttonName == "Rating") {
      if ((courtType == "FirstInstance" && !defendant )
      || (courtType == "FirstInstance" && defendant && this.requiredRating))
        if (
          this.authService.isAdministratorOfJudiciary &&
          (stageName == "NewCase" || stageName == "Rated")
        )
          return true;

    }
    ////////  Accept  ////////
    else if (buttonName == "Accept") {
      if ((courtType == "FirstInstance" && !defendant )
      || (courtType == "FirstInstance" && defendant && this.requiredRating))
        if (
          this.authService.isHeadOfJudiciary &&
          (stageName == "Rated" || stageName == "Rejected")
        )
          return true;
    }
    //////// Reject ////////
    else if (buttonName == "Reject") {
      if ((courtType == "FirstInstance" && !defendant )
      || (courtType == "FirstInstance" && defendant && this.requiredRating))
        if (
          this.authService.isHeadOfJudiciary &&
          (stageName == "Rated" || stageName == "Accepted")
        )
          return true;
    }
    /////// AssignConsultant ////////
    else if (buttonName == "AssignConsultant") {
      if (
        this.authService.isAdministratorOfJudiciary &&
        (stageName == "Accepted" ||
          stageName == "ConsultantAssigned" ||
          stageName == "InfoCompleted" ||
          stageName == "HearingStarted" ||
          stageName == "HearingClosed")
      )
        return true;
    }
    //////// AddHearing ////////
    else if (buttonName == "AddHearing") {
      if (
        this.authService.isConsultant &&
        (stageName == "InfoCompleted" || stageName == "HearingClosed")
      )
        return true;
    }
    //////// Judgement ////////
    else if (buttonName == "Judgement") {
      if (
        this.authService.currentUser?.id == consultantId &&
        (stageName == "HearingClosed" || stageName == "CaseJudgement")
      )
        return true;
    }
    //////// ChangeConsultantRequest ////////
    else if (buttonName == "ChangeConsultantRequest") {
      if (
        this.authService.currentUser?.id == consultantId &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
    }

    //////// CloseCase ////////
    else if (buttonName == "CloseCase") {
      if (
        this.authService.isAdministratorOfJudiciary &&
        stageName == "CaseJudgement"
      )
        return true;
    }
    //////// OpenCase ////////
    else if (buttonName == "OpenCase") {
      if (
        this.authService.currentUser?.id == consultantId &&
        stageName == "Closed"
      )
        return true;
    } */

    return false;
  }
}
