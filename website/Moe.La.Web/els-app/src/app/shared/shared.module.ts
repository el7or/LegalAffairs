import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {  RxReactiveFormsModule } from "@rxweb/reactive-form-validators"

import { MenuItems } from './menu-items/menu-items';
import {
  AccordionAnchorDirective,
  AccordionLinkDirective,
  AccordionDirective,
} from './accordion';
import { ConsultationButtonNamePipe } from './pipes/consultation-button-name.pipe';
import { HearingShowButtonPipe } from './pipes/hearing-show-button.pipe';
import { NumbersOnlyValidatorDirective } from './validators/numbers-only-validator.directive';
import { MatchValidatorDirective } from './validators/match-validator.directive';
import { MobileValidatorDirective } from './validators/mobile-validator.directive';
import { ValidNameValidator } from './validators/valid-name-validator.directive';
import { MobileValidator2Directive } from './validators/mobile-validator2.directive';
import { ArrayKeyValuePipe } from './pipes/array-key-value.pipe';
import { DaysDiffPipe } from './pipes/days-diff.pipe';
import { FirstLastNamePipe } from './pipes/first-last-name.pipe';
import { EmptyNameReplacementPipe } from './pipes/empty-name-replacement.pipe';
import { SubTitlePipe } from './pipes/sub-title.pipe';
import { ArTypeNamePipe } from './pipes/ar-type-name.pipe';
import { ArSatgeNamePipe } from './pipes/ar-satge-name.pipe';
import { CaseShowButtonPipe } from './pipes/case-show-button.pipe';
import { CaseButtonNamePipe } from './pipes/case-button-name.pipe';
import { LoaderComponent } from './components/loader/loader.component';
import { ArLegalMemoStatusPipe } from './pipes/ar-legal-memo-status.pipe';
import { RolesListPipe } from './pipes/roles-list.pipe';
import { HijriConverterPipe } from './pipes/hijri-converter.pipe';
import { AngularMaterialComponentsModule } from './modules/angular-material-components.module';
import { SliceWordsPipe } from './pipes/slice-words.pipe';
import { ArCaseStatusPipe } from './pipes/ar-case-status.pipe';
import { ArCaseSourcePipe } from './pipes/ar-case-source.pipe';
import { ArLitigationTypePipe } from './pipes/ar-litigation-type.pipe';
import { ArMinistryLegalStatusPipe } from './pipes/ar-ministry-legal-status.pipe';
import { ArrayNamesPipe } from './pipes/array-names.pipe';
import { ArrayValuesPipe } from './pipes/array-values.pipe';
import { ErrorDialogComponent } from './components/error-dialog/error-dialog.component';
import { GetPageDataPipe } from './pipes/get-page-data.pipe';
import { ArDayOfWeek } from './pipes/ar-day-of-week.pipe';
import { DigitsOnlyDirective } from './directives/digits-only.directive';
import { LettersOnlyDirective } from './directives/letters-only.directive';
import { BlockCopyPasteDirective } from './directives/appBlockCopyPaste';
import { ReplaceLineBreaksPipe } from './pipes/replace-line-breaks.pipe';
import { DragDropDirective } from './directives/drag-drop.directive';
import { UploadComponent } from './components/attachments/upload/upload.component';
import { AttachmentsComponent } from './components/attachments/attachments.component';
import { ArabicOnlyDirective } from './directives/arabic-only.directive';


const MODULES = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  RxReactiveFormsModule,
  HttpClientModule,
  AngularMaterialComponentsModule,
  FlexLayoutModule
];
const COMPONENTS = [
  LoaderComponent,
  ErrorDialogComponent,
  AttachmentsComponent,
  UploadComponent
];

const DIRECTIVES = [
  DragDropDirective,
  NumbersOnlyValidatorDirective,
  MatchValidatorDirective,
  MobileValidatorDirective,
  MobileValidator2Directive,
  ValidNameValidator,
  AccordionAnchorDirective,
  AccordionLinkDirective,
  AccordionDirective,
  DigitsOnlyDirective,
  LettersOnlyDirective,
  BlockCopyPasteDirective,
  ArabicOnlyDirective
];

const PIPES = [
  ArrayKeyValuePipe,
  DaysDiffPipe,
  EmptyNameReplacementPipe,
  FirstLastNamePipe,
  SubTitlePipe,
  HearingShowButtonPipe,
  ConsultationButtonNamePipe,
  ArTypeNamePipe,
  ArSatgeNamePipe,
  CaseShowButtonPipe,
  CaseButtonNamePipe,
  ArLegalMemoStatusPipe,
  RolesListPipe,
  HijriConverterPipe,
  SliceWordsPipe,
  ArCaseStatusPipe,
  ArCaseSourcePipe,
  ArLitigationTypePipe,
  ArMinistryLegalStatusPipe,
  ArrayNamesPipe,
  ArrayValuesPipe,
  GetPageDataPipe,
  ReplaceLineBreaksPipe,
  ArDayOfWeek
];

@NgModule({
  imports: [...MODULES],
  declarations: [...COMPONENTS, ...DIRECTIVES, ...PIPES, GetPageDataPipe, ReplaceLineBreaksPipe],
  exports: [...COMPONENTS, ...DIRECTIVES, ...PIPES, ...MODULES],
  // entryComponents: [LoaderComponent],

  providers: [MenuItems],
})
export class SharedModule {}
