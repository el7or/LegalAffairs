import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arLitigationType'
})
export class ArLitigationTypePipe implements PipeTransform {

  transform(value: string): unknown {        
    if (this.statusTranslator.find(t => t.value == value))
      return this.statusTranslator.find(t => t.value == value )?.ar;
  }

  statusTranslator = [
    { en: "FirstInstance", ar: "ابتدائية",value:'1' },
    { en: "Appeal", ar: "استئناف",value:'2' },
    { en: "Supreme", ar: "عليا",value:'3' },   
  ]
}
