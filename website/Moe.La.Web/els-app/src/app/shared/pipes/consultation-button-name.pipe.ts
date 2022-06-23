import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'consultationButtonName'
})
export class ConsultationButtonNamePipe implements PipeTransform {

    transform(stageName: string, buttonName: string): string {
        if (buttonName == 'AssignConsultant') {
            if (stageName == 'NewConsultation')
                return 'تكليف مستشار';
            else if (stageName == 'ConsultantAssigned')
                return 'تغيير المستشار المكلف';
            return stageName;
        }
        else if (buttonName == 'LegalOpinion') {
            if (stageName == 'ConsultantAssigned'
                || stageName == 'FilesRequested'
                || stageName == 'FilesSubmitted'
                || stageName == 'AdministratorReturn'
                || stageName == 'GMReturn')
                return 'الراي القانوني';
            else if (stageName == 'LegalOpinionSubmitted')
                return 'تعديل الراي القانوني';
            return stageName;
        }
        else
            return stageName;
    }
}
