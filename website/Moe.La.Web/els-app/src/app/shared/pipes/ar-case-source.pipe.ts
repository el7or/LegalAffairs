import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arCaseSource'
})
export class ArCaseSourcePipe implements PipeTransform {

  transform(value: string): unknown {        
    if (this.statusTranslator.find(t => t.value == value))
      return this.statusTranslator.find(t => t.value == value )?.ar;
  }

  statusTranslator = [
    { en: "Najiz", ar: "ناجز",value:'1' },
    { en: "Moeen", ar: "معين",value:'2' },
    { en: "QuasiJudicialCommittee", ar: "اللجان شبة قضائية",value:'3' },   
  ]
}
