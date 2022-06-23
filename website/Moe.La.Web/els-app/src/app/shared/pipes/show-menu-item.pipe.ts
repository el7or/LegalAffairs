import { Pipe, PipeTransform } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { AppSettingsService } from '../../core/services/app-settings.service';

@Pipe({
  name: 'showMenuItem'
})
export class showMenuItemPipe implements PipeTransform {

  constructor(private authService: AuthService, private appSettingsService: AppSettingsService) { }

  transform(menuItem: string): boolean {

/*     if (menuItem == 'adminMenuItem') {
      if (this.authService.isAdmin)
        return true;
    }
    else if (menuItem == 'caseMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isConsultant
        || this.authService.isSecretary)
        return true;
    }
    else if (menuItem == 'appealMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isConsultant)
        return true;
    }
    else if (menuItem == 'hearingMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isConsultant)
        return true;
    }
    else if (menuItem == 'hearingRequestFileMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isConsultant
        || this.authService.isSecretary)
        return true;
    }
    else if (menuItem == 'customerMenuItem') {
      if ((this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isSecretary) && this.appSettingsService.isLawFirmOffice)
        return true;
    }
    else if (menuItem == 'agencyMenuItem') {
      if ((this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isSecretary) && this.appSettingsService.isLawFirmOffice)
        return true;
    }
    else if (menuItem == 'adversaryMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isSecretary)
        return true;
    }
    else if (menuItem == 'caseReportMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isConsultant)
        return true;
    }
    else if (menuItem == 'investigationMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isConsultant
        || this.authService.isSecretary
        || this.authService.isOfficer)
        return true;
    }
    else if (menuItem == 'executiveMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfJudiciary
        || this.authService.isSecretary
        || this.authService.isConsultant)
        return true;
    }
    else if (menuItem == 'consultationMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfContracts
        || this.authService.isConsultant
        || this.authService.isSecretary)
        return true;
    }
    else if (menuItem == 'ConsultationRequestFileMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isConsultant
        || this.authService.isSecretary)
        return true;
    }
    else if (menuItem == 'consultationReportMenuItem') {
      if (this.authService.isAdmin
        || this.authService.isHeadOfJudiciary
        || this.authService.isAdministratorOfContracts
        || this.authService.isConsultant)
        return true;
    } */

    return false;
  }

}
