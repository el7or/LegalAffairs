
import { Pipe, PipeTransform } from "@angular/core";
import { AuthService } from '../../../app/core/services/auth.service';

@Pipe({
  name: "hearingShowButton",
})
export class HearingShowButtonPipe implements PipeTransform {
  constructor(private authService: AuthService) {}

  transform(
    buttonName: string,
    stageName: string,
    consultantId: string,
    hearingClosed: boolean
  ): boolean {
    /* if (buttonName == "Edit") {
      if (
        this.authService.currentUser?.id == consultantId &&
        stageName != "CaseJudgement" &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
    } else if (buttonName == "Delete") {
      if (
        this.authService.isAdmin ||
        (this.authService.isAdministratorOfJudiciary &&
          stageName != "CaseJudgement" &&
          stageName != "Appealed" &&
          stageName != "Closed")
      )
        return true;
    } else if (buttonName == "NewProcedure") {
      if (
        (this.authService.currentUser?.id == consultantId ||
          this.authService.isHeadOfJudiciary ||
          this.authService.isAdministratorOfJudiciary) &&
        !hearingClosed &&
        stageName != "CaseJudgement" &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
    } else if (buttonName == "NewRequestFile") {
      if (
        (this.authService.currentUser?.id == consultantId ||
          this.authService.isHeadOfJudiciary ||
          this.authService.isAdministratorOfJudiciary) &&
        !hearingClosed &&
        stageName != "CaseJudgement" &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
    } else if (buttonName == "Close") {
      if (
        this.authService.currentUser?.id == consultantId &&
        stageName != "CaseJudgement" &&
        stageName != "Appealed" &&
        stageName != "Closed"
      )
        return true;
    } */

    return false;
  }
}
