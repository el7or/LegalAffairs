import { Injectable } from '@angular/core';

import { Department } from 'app/core/enums/Department';
import { AppRole, RoleDepartments } from 'app/core/models/role';

export interface BadgeItem {
  type: string;
  value: string;
}

export interface Saperator {
  name: string;
  type?: string;
}

export interface SubChildren {
  state: string;
  name: string;
  type?: string;
  rolesDepartment?: RoleDepartments[];
}

export interface ChildrenItems {
  state: string;
  name: string;
  type?: string;
  child?: SubChildren[];
  badge?: BadgeItem[];
  rolesDepartment?: RoleDepartments[];
}

export interface Menu {
  state: string;
  name: string;
  type: string;
  icon: string;
  badge?: BadgeItem[];
  saperator?: Saperator[];
  children?: ChildrenItems[];
  rolesDepartment?: RoleDepartments[];
}

const MENUITEMS = [
  {
    state: 'home',
    name: 'الرئيسية',
    type: 'link',
    icon: 'home',
  },
  /*  {
     state: 'moamala-rasel-inbox', // url routing
     name: 'الصندوق الوارد لراسل',
     type: 'link',
     icon: 'inbox',
     rolesDepartment: [
       { role: AppRole.Distributor },
       { role: AppRole.GeneralSupervisor },
     ]
   }, */
  {
    state: '',
    name: 'إدارة خدمات الترافع الالكتروني',
    type: 'saperator',
    icon: '',
    rolesDepartment: [
      { role: AppRole.GeneralSupervisor },
      { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
      { role: AppRole.AdministrativeCommunicationSpecialist },
      { role: AppRole.BranchManager },
      { role: AppRole.RegionsSupervisor },
      { role: AppRole.MainBoardHead },
      { role: AppRole.SubBoardHead },
      { role: AppRole.DataEntry },
    ]
  },
  {
    state: 'requests', // url routing
    name: 'طلباتي',
    type: 'link',
    icon: 'featured_play_list',
    rolesDepartment: [
      { role: AppRole.GeneralSupervisor },
      { role: AppRole.RegionsSupervisor },
      { role: AppRole.BranchManager },
      { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
      { role: AppRole.AdministrativeCommunicationSpecialist },
    ]
  },
  {
    state: 'cases', // url routing
    name: 'القضايا',
    type: 'link',
    icon: 'work',
    rolesDepartment: [
      { role: AppRole.BranchManager },
      { role: AppRole.GeneralSupervisor },
      { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
      { role: AppRole.RegionsSupervisor },
      { role: AppRole.DataEntry },
    ]
  },
  {
    state: 'hearings/', // url routing
    name: 'الجلسات',
    type: 'link',
    icon: 'event_note',
    rolesDepartment: [
      { role: AppRole.BranchManager },
      { role: AppRole.GeneralSupervisor },
      { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
      { role: AppRole.RegionsSupervisor },
    ]
  },
  {
    state: 'memos', // url routing
    name: 'المذكرات',
    type: 'sub',
    icon: 'sticky_note_2',
    children: [
      {
        state: 'list', name: 'المذكرات', type: 'link',
        rolesDepartment: [
          { role: AppRole.AllBoardsHead },
          { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
        ]
      },
      {
        state: 'review-list',
        name: 'مذكرات للمراجعة',
        type: 'link',
        rolesDepartment: [
          { role: AppRole.SubBoardHead },
          { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] }
        ]
      },
      {
        state: 'board-review-list',
        name: 'المذكرات الواردة',
        type: 'link',
        rolesDepartment: [
          { role: AppRole.MainBoardHead }
        ]
      },
      {
        state: 'meetings', name: 'اجتماعات اللجان', type: 'link',
        rolesDepartment: [
          { role: AppRole.MainBoardHead },
          { role: AppRole.SubBoardHead },
          { role: AppRole.BoardMember }
        ]
      },
    ],
    rolesDepartment: [
      { role: AppRole.MainBoardHead },
      { role: AppRole.SubBoardHead },
      { role: AppRole.AllBoardsHead },
      { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
    ]

  },
  {
    state: 'legal-boards', // url routing
    name: 'لجان المذكرات',
    type: 'sub',
    icon: 'meeting_room',
    children: [{ state: 'list', name: 'تشكيل اللجان', type: 'link' }],
    rolesDepartment: [
      { role: AppRole.MainBoardHead }]
  },
  // {
  //   state: 'moamalat', // url routing
  //   name: 'المعاملات',
  //   type: 'link',
  //   icon: 'forum',
  //   rolesDepartment: [
  //     { role: AppRole.GeneralSupervisor },
  //     { role: AppRole.Distributor },
  //     { role: AppRole.BranchManager },
  //     { role: AppRole.DepartmentManager, departmentIds: [Department.All] },
  //     { role: AppRole.LegalConsultant, departmentIds: [Department.All] },
  //     { role: AppRole.LegalResearcher, departmentIds: [Department.All] },
  //     { role: AppRole.Investigator, departmentIds: [Department.Investiation] }
  //   ]
  // },
  /* {
    state: 'tasks', // url routing
    name: 'المهام',
    type: 'sub',
    icon: 'rule',
    badge: [{ type: 'red', value: '7' }],
    children: [
      { state: 'waiting', name: 'مهام غير منجزة', type: 'link' },
      { state: 'completed', name: 'مهام منجزة', type: 'link' },
    ],
  },
  {
    state: 'parties', // url routing
    name: 'الخصوم',
    type: 'link',
    icon: 'reduce_capacity',
  },
  {
    state: 'reports', // url routing
    name: 'التقارير',
    type: 'sub',
    icon: 'insert_chart_outlined',
    children: [
      { state: 'cases', name: 'تقرير القضايا', type: 'link' },
      { state: 'hearings', name: 'تقرير الجلسات', type: 'link' },
      { state: 'tasks', name: 'تقرير المهام', type: 'link' },
    ],
  }, */
  // {
  //   state: '',
  //   name: 'إدارة خدمات التحقيقات',
  //   type: 'saperator',
  //   icon: '',
  //   // roles: [
  //   //   AppRole.Investigator,
  //   //   AppRole.InvestigationManager,
  //   //   AppRole.GeneralSupervisor,
  //   // ],
  //   rolesDepartment: [
  //     { role: AppRole.Investigator },
  //     { role: AppRole.GeneralSupervisor },
  //     { role: AppRole.DepartmentManager, departmentIds: [Department.Investiation] }
  //   ]
  // },
  // {
  //   state: 'investigations', // url routing
  //   name: 'التحقيقات',
  //   type: 'link',
  //   icon: 'gavel',
  //   // roles: [
  //   //   AppRole.Investigator,
  //   //   AppRole.InvestigationManager,
  //   //   AppRole.GeneralSupervisor,
  //   // ],
  //   rolesDepartment: [
  //     { role: AppRole.Investigator },
  //     { role: AppRole.GeneralSupervisor },
  //     { role: AppRole.DepartmentManager, departmentIds: [Department.Investiation] }
  //   ]
  // },
  // {
  //   state: 'investigation-records', // url routing
  //   name: 'محاضر التحقيقات',
  //   type: 'link',
  //   icon: 'event_note',
  //   // roles: [AppRole.Investigator],
  //   rolesDepartment: [
  //     { role: AppRole.Investigator }
  //   ]
  // },
  // {
  //   state: '',
  //   name: 'الإدارات الاستشارية',
  //   type: 'saperator',
  //   icon: '',
  //   rolesDepartment: [
  //     { role: AppRole.LegalResearcher, departmentIds: [Department.All] },
  //     { role: AppRole.GeneralSupervisor, departmentIds: null },
  //     { role: AppRole.Distributor, departmentIds: null },
  //     { role: AppRole.DepartmentManager, departmentIds: [Department.All] }
  //   ]
  // },
  // {
  //   state: 'consultation', // url routing
  //   name: 'النماذج',
  //   type: 'link',
  //   icon: 'wysiwyg',
  //   rolesDepartment: [
  //     { role: AppRole.LegalResearcher, departmentIds: [Department.All] },
  //     { role: AppRole.GeneralSupervisor, departmentIds: null },
  //     { role: AppRole.Distributor, departmentIds: null },
  //     { role: AppRole.DepartmentManager, departmentIds: [Department.All] }
  //   ]
  // },
  {
    state: '',
    name: 'إدارة النظام',
    type: 'saperator',
    icon: '',
    rolesDepartment: [
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
      { role: AppRole.Admin },
    ]
  },
  {
    state: 'pleading', // url routing
    name: 'الترافع',
    type: 'sub',
    icon: 'wysiwyg',
    children: [
      {
        state: 'researchers-consultants',
        name: 'ربط الباحثين بالمستشار',
        type: 'link',
        icon: 'supervised_user_circle',
        rolesDepartment: [
          { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] }
        ]
      },
    ],
    rolesDepartment: [
      { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] }
    ]
  },
  {
    state: 'administration-management', // url routing
    name: 'البيانات الأساسية',
    type: 'sub',
    icon: 'settings',
    //badge: [{ type: 'warning', value: 'Admin' }],
    children: [
      { state: 'case-categories', name: 'تصنيف القضية', type: 'link' },
      { state: 'job-titles', name: 'المسميات الوظيفية', type: 'link' },
      {
        state: 'branches',
        name: 'الفروع',
        type: 'link',
      },
      {
        state: 'department',
        name: 'الإدارات المتخصصة',
        type: 'link',
      },
      { state: 'courts', name: 'المحاكم', type: 'link' },
      // { state: 'case-ratings', name: 'التقييمات', type: 'link' },
      // { state: 'case-types', name: 'أنواع القضايا', type: 'link' },
      //{ state: 'party-types', name: 'أنواع الخصوم', type: 'link' },
      //{ state: 'identity-types', name: 'أنواع الهوية', type: 'link' },
      //{ state: 'field-mission-types', name: 'المهام الميدانية', type: 'link' },
      { state: 'attachment-types', name: 'أنواع المرفقات', type: 'link' },
      { state: 'ministry-sectors', name: 'قطاعات الوزارة', type: 'link' },
      { state: 'ministry-departments', name: 'إدارات الوزارة', type: 'link' },
      // {
      //   state: 'investigation-record-party-type',
      //   name: 'أنواع الطرف',
      //   type: 'link',
      // },
      { state: 'cities', name: 'المدن', type: 'link' },
      { state: 'provinces', name: 'المناطق', type: 'link' },
      { state: 'district', name: 'الأحياء', type: 'link', },
      {
        state: 'governmentOrganization',
        name: 'الجهات',
        type: 'link',
      },
      // {state: 'investigation-record-party-type',name: 'أنواع الطرف',type: 'link',},
      // {
      //   state: 'investigation-questions',
      //   name: 'أسئلة التحقيقات',
      //   type: 'link',
      // },
      // {
      //   state: 'work-item-type',
      //   name: 'أنواع العمل',
      //   type: 'link',
      // },
      // {
      //   state: 'sub-work-item-type',
      //   name: 'أنواع العمل الفرعية',
      //   type: 'link',
      // },
      // { state: 'researchers', name: 'توزيع الباحثين', type: 'link' },
    ],
    rolesDepartment: [
      { role: AppRole.Admin }
    ]
  },
  {
    state: 'users', // url routing
    name: 'المستخدمين',
    type: 'link',
    icon: 'admin_panel_settings',
    rolesDepartment: [
      { role: AppRole.Admin }
    ]
  },
];

@Injectable()
export class MenuItems {
  getMenuitem(): Menu[] {
    return MENUITEMS;
  }
}
