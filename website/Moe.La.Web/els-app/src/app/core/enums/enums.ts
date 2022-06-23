import { EnumValue } from '../models/enum-value';

export abstract class Enums {

    static readonly courtCategries: EnumValue[] = [
        { value: 1, name: 'MinistryOfJustice', nameAr: 'وزارة العدل' },
        { value: 2, name: 'HouseOfGrievances', nameAr: 'ديوان المظالم' },
        { value: 3, name: 'QuasiJudicialCommittees', nameAr: 'لجان شبة قضائية' },
    ];

    static readonly courtType: EnumValue[]  = [
        { value: 1, name: 'FirstInstance', nameAr: 'محكمة ابتدائية' },
        { value: 2, name: 'Appeal', nameAr: 'محكمة استئناف' },
        { value: 3, name: 'Supreme', nameAr: 'محكمة عليا' },
    ];

    static readonly  RequestStatus:EnumValue[]=[
      { value: 1, name: 'Waiting', nameAr: 'بالانتظار' },
      { value: 2, name: 'Accepted', nameAr: 'موافقة' },
      { value: 3, name: 'Rejected', nameAr: 'رفض' },

    ]

    static readonly  LegalMemoStatus :EnumValue[]=[
        { value: 1, name: 'New', nameAr: 'جديد' },
        { value: 2, name: 'Unactivated', nameAr: 'معطلة' },
        { value: 3, name: 'Approved', nameAr: 'معتمدة من اللجنة' },
        { value: 4, name: 'Returned', nameAr: 'مردودة لإعادة الصياغة' },
        { value: 5, name: 'Rejected', nameAr: 'مرفوضة' },
        { value: 6, name: 'Accepted', nameAr: 'مقبولة' },
        { value: 7, name: 'RaisingConsultant', nameAr: 'رفع للمستشار' },
        { value: 8, name: 'RaisingMainBoardHead ', nameAr: 'محولة إلى لجنة رئيسية' },
        { value: 9, name: 'RaisingSubBoardHead ', nameAr: 'محولة إلى لجنة فرعية' }
      ]
}
