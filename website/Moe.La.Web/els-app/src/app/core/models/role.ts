export interface Role {
  id: string;
  name: string;
  nameAr: string;
  priority: number;
}


export enum AppRole {
  /**
   * مدير النظام
   */
  Admin = 'Admin',
  /**
   * المشرف العام
   */
  GeneralSupervisor = 'GeneralSupervisor',
  /**
   * مشرف المناطق
   */
  RegionsSupervisor = 'RegionsSupervisor',
  /**
   * مدير منطقة / إدارة تعليمية
   */
  BranchManager = 'BranchManager',
  /**
    * مدير الإدارة المختصة
    */
  DepartmentManager = 'DepartmentManager',
  /**
   * رئيس كل اللجان
   */
   AllBoardsHead = 'AllBoardsHead',
  /**
   * رئيس لجنة رئيسية
   */
   MainBoardHead = 'MainBoardHead',
  /**
   * رئيس لجنة فرعية
   */
   SubBoardHead = 'SubBoardHead',
  /**
   * عضو لجنة
   */
  BoardMember = 'BoardMember',
  /**
   * مستشار قانوني
   */
  LegalConsultant = 'LegalConsultant',
  /**
   * باحث قانوني
   */
  LegalResearcher = 'LegalResearcher',
  /**
   * موزع المعاملات
   */
  Distributor = 'Distributor',
  /**
   * محقق
   */
  Investigator = 'Investigator',

  /**
   * مختص الاتصالات الادارية
   */
  AdministrativeCommunicationSpecialist = 'AdministrativeCommunicationSpecialist',

  /**
   * مدخل بيانات
   */
   DataEntry = 'DataEntry',

}

export interface RoleDepartments {
  role: AppRole,
  departmentIds?: number[]
}
