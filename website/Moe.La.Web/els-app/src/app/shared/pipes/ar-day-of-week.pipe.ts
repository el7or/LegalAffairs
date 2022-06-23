import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'arDayOfWeek'
})
export class ArDayOfWeek implements PipeTransform {

  transform(date:Date): string {
    var day= new Date(date).getDay();

    if (this.dayTranslator.find(t => t.value == day))
      return this.dayTranslator.find(t => t.value == day )?.ar;
  }

  dayTranslator = [
    { en: "Sunday", ar: "الأحد",value:0},
    { en: "Monday", ar: "الاثنين",value:1 },
    { en: "Tuesday", ar: "الثلاثاء",value:2 },   
    { en: "Wednesday", ar: "الأربعاء",value:3 },   
    { en: "Thursday", ar: "الخميس",value:4 },  
    { en: "Friday", ar: "الجمعة",value:5 },   
    { en: "Saturday", ar: "السبت",value:6 },   
  ]
}
