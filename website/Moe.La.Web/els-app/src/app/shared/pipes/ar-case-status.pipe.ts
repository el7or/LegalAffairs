import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arCaseStatus'
})
export class ArCaseStatusPipe implements PipeTransform {

  transform(enName: string): unknown {
    if (this.statusTranslator.find(t => t.en == enName||t.id==enName))
      return this.statusTranslator.find(t => t.en == enName||t.id==enName)?.ar;
  }

  statusTranslator = [
    { en: "IncomingCase", ar: "قضية واردة",id:'0' },
    { en: "NewCase", ar: "قضية جديدة",id:'1' },
    { en: "ReceivedByResearcher", ar: "مستلمة من الباحث",id:'2' },
    { en: "ClosedCase", ar: "قضية مغلقة",id:'30' },
    { en: "DoneElementaryJudgment", ar: "تم نطق الحكم الابتدائي",id:'3' },
    { en: "DoneAppellateJudgment", ar: "تم نطق حكم الاستئناف",id:'4' },
    {en:"DoneSupremeJudgment",ar:"تم نطق حكم العليا",id:'5'},
    {en:"ReceivedByLitigationManager",ar:"مستلمة من مدير الترافع",id:'6'},
    {en:"SentToRegionsSupervisor",ar:"واردة إلى مشرف المناطق",id:'7'},
    {en:"ReceivedByRegionsSupervisor",ar:"مستلمة من مشرف المناطق",id:'8'},
    {en:"SentToBranchManager",ar:"واردة إلى مدير المنطقة",id:'9'},
    {en:"ReceivedByBranchManager",ar:"مستلمة من مدير المنطقة",id:'10'},
    {en:"ReturnedToRegionsSupervisor",ar:"معادة إلى مشرف المناطق",id:'11'}
  ]
}
