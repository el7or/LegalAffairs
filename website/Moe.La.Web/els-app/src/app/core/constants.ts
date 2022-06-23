export abstract class Constants {
  static readonly BASE_URL: string = 'https://localhost:44387/'; //;'https://las-uat.moe.gov.sa/'; //
  static readonly API_URL: string = Constants.BASE_URL + 'api/';

  // ADFS
  static readonly ADFS_AUTHORITY_URL = 'https://stsstaging.moe.gov.sa/adfs';
  static readonly ADFS_CLIENT_ID = 'f22e60b6-9157-4b64-b7a1-3e03f1aed7d0';

  // Endpoints
  static readonly CASE_API_URL: string = Constants.API_URL + 'cases';
  static readonly PARTY_API_URL: string = Constants.API_URL + 'parties';
  static readonly CASE_CATEGORY_API_URL: string = Constants.API_URL + 'CaseCategories';
  static readonly SECOND_SUB_CATEGORY_API_URL: string = Constants.API_URL + 'second-sub-categories';
  static readonly First_SUB_CATEGORY_API_URL: string = Constants.API_URL + 'first-sub-categories';
  static readonly MAIN_CATEGORY_API_URL: string = Constants.API_URL + 'main-categories';
  static readonly DISTRICT_API_URL: string = Constants.API_URL + 'districts';
  static readonly GOVERNMENT_ORGANIZATION_API_URL: string = Constants.API_URL + 'government-organizations';
  static readonly REQUEST_API_URL: string = Constants.API_URL + 'requests';
  static readonly TRANSACTION_API_URL: string = Constants.API_URL + 'transaction';
  static readonly COURT_API_URL: string = Constants.API_URL + 'courts';
  static readonly JOBTITLE_API_URL: string = Constants.API_URL + 'jobs';
  static readonly CASERATING_API_URL: string = Constants.API_URL + 'case-ratings';
  static readonly CITY_API_URL: string = Constants.API_URL + 'cities';
  static readonly FIELDMISSIONTYPE_API_URL: string = Constants.API_URL + 'field-mission-types';
  static readonly BRANCH_API_URL: string = Constants.API_URL + 'branches';
  static readonly IDENTITYTYPE_API_URL: string = Constants.API_URL + 'identity-types';
  static readonly PARTYTYPE_API_URL: string = Constants.API_URL + 'party-types';
  static readonly PROVINCE_API_URL: string = Constants.API_URL + 'provinces';
  static readonly WORK_ITEM_TYPE_API_URL: string = Constants.API_URL + 'work-item-type';
  static readonly LEGALMEMO_API_URL: string = Constants.API_URL + 'legal-memos';
  static readonly USER_API_URL: string = Constants.API_URL + 'users';
  static readonly LEGALBOARD_API_URL: string = Constants.API_URL + 'legal-boards';
  static readonly BOARD_MEETING_API_URL: string = Constants.API_URL + 'legal-boards/board-meeting';
  static readonly HEARINGS_API_URL: string = Constants.API_URL + 'hearings';
  static readonly WORKFLOW_CONFIGURATION_API_URL: string = Constants.API_URL + 'workflow-configuration';
  static readonly WORKFLOW_LOOKUPS_API_URL: string = Constants.API_URL + 'workflow-lookups';
  static readonly RESEARCHER_CONSULTANT_API_URL: string = Constants.API_URL + 'researcher-consultant';
  static readonly ATTACHMENT_CONSULTANT_API_URL: string = Constants.API_URL + 'attachments';
  static readonly NOTIFICATION_API_URL: string = Constants.API_URL + 'system-notifications';
  static readonly ATTACHMENTTYPE_API_URL: string = Constants.API_URL + 'attachment-types';
  static readonly AD_API_URL: string = Constants.API_URL + 'integration/ad';
  static readonly MOAMALATTRANSACTION_API_URL: string = Constants.API_URL + 'moamalat-transactions';
  static readonly MINISTRYDEPARTMENT_API_URL: string = Constants.API_URL + 'ministry-departments';
  static readonly MINISTRYSECTOR_API_URL: string = Constants.API_URL + 'ministry-sectors';
  static readonly DEPARTMENT_API_URL: string = Constants.API_URL + 'departments';
  static readonly INVESTIGATION_RECORD_API_URL: string = Constants.API_URL + 'investigation-records';
  static readonly INVESTIGATION_API_URL: string = Constants.API_URL + 'investigations';
  static readonly FARES_API_URL: string = Constants.API_URL + 'integration/faris';
  static readonly Investigation_Question_API_URL: string = Constants.API_URL + 'investigation-questions';
  static readonly MOAMALAT_API_URL: string = Constants.API_URL + 'moamalat';
  static readonly MOAMALAT_RASEL_INBOX_API_URL: string = Constants.API_URL + 'integration/moamala-log'
  static readonly SUB_WORK_ITEM_TYPE_API_URL: string = Constants.API_URL + 'sub-work-item-type';
  static readonly CONSULTATION_API_URL: string = Constants.API_URL + 'consultations';
  static readonly LETTER_TEMPLATE_API_URL: string = Constants.API_URL + 'lettertemplates';
  static readonly REQUEST_LETTER_API_URL: string = Constants.API_URL + 'requestletters';
}
