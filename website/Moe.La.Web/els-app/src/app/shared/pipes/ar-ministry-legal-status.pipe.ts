import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arMinistryLegalStatus'
})
export class ArMinistryLegalStatusPipe implements PipeTransform {

  transform(value: string): unknown {        
    if (this.statusTranslator.find(t => t.value == value))
      return this.statusTranslator.find(t => t.value == value )?.ar;
  }

  statusTranslator = [
    { en: "Defendant", ar: "مدعى عليها",value:'1' },
    { en: "Applicant", ar: "مدعية",value:'2' }    
  ]
}
