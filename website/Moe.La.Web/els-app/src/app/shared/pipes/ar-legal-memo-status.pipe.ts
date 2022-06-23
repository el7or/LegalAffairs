import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arLegalMemoStatus'
})
export class ArLegalMemoStatusPipe implements PipeTransform {

  transform(enName: string): unknown {
    if (this.statusTranslator.find(t => t.en == enName||t.id==enName))
      return this.statusTranslator.find(t => t.en == enName||t.id==enName)?.ar;
  }

  statusTranslator = [
    { en: "New", ar: "جديدة",id:'1' },
    { en: "Unactivated", ar: "معطلة",id:'2' },
    { en: "Approved", ar: "معتمدة من اللجنة",id:'3' },
    { en: "Returned", ar: "مردودة لإعادة الصياغة",id:'4' },
    { en: "Rejected", ar: "مرفوضة",id:'5' },
    { en: "Accepted", ar: "مقبولة من المستشار",id:'6' },
    {en:"RaisingConsultant",ar:"مرفوعة للمستشار",id:'7'},
    {en:"RaisingMainBoardHead ",ar:"محولة إلى لجنة  رئيسية",id:'8'},
    {en:"RaisingSubBoardHead ",ar:"محولة إلى لجنة  فرعية",id:'9'}
  ]
}
