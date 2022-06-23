import { Location } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import '@ckeditor/ckeditor5-build-classic/build/translations/ar';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { QueryObject, LegalMemoNoteQueryObject, CaseQueryObject } from 'app/core/models/query-objects';
import { MemoQueryObject } from 'app/core/models/query-objects';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { AuthService } from 'app/core/services/auth.service';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { LegalMemoNote } from 'app/core/models/legal-memo-note';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseService } from 'app/core/services/case.service';
import { MatDialog } from '@angular/material/dialog';
import { CaseListItem } from 'app/core/models/case';
import { HearingSearchCasesComponent } from '../../hearings/hearing-search-cases/hearing-search-cases.component';

@Component({
  selector: 'app-legal-memo-form',
  templateUrl: './legal-memo-form.component.html',
  styleUrls: ['./legal-memo-form.component.css'],
})
export class LegalMemoFormComponent implements OnInit, OnDestroy {
  id: number = 0;
  caseId: number = 0;
  caseNumberInSource?: string;
  legalMemoStatus: any;
  queryObject: QueryObject = new QueryObject();
  caseQueryObject: CaseQueryObject = new CaseQueryObject();
  memoStatus: LegalMemoStatus;
  memoQueryObject: MemoQueryObject = new MemoQueryObject();
  noteQueryObject: LegalMemoNoteQueryObject = new LegalMemoNoteQueryObject({
    sortBy: 'createdOn',
    pageSize: 999,
  });

  dataSource!: MatTableDataSource<LegalMemoNote>;
  mainCategory: string = "";
  firstSubCategory: string = "";
  secondSubCategory: string = "";
  caseSubject: string = "";
  legalMemoTypes: any = [];

  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  form: FormGroup = Object.create(null);

  private subs = new Subscription();
  public Editor = CustomEditor;
  public config = {
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'underline',
        'link',
        'bulletedList',
        'numberedList',
        '|',
        'indent',
        'outdent',
        '|',
        'blockQuote',
        'insertTable',
        'undo',
        'redo',
        '|',
        "alignment:left", "alignment:right", "alignment:center"
      ]
    },
    image: {
      toolbar: [
        'imageStyle:full',
        'imageStyle:side',
        '|',
        'imageTextAlternative'
      ]
    },
    table: {
      contentToolbar: [
        'tableColumn', 'tableRow', 'mergeTableCells'
      ]
    },
    // This value must be kept in sync with the language defined in webpack.config.js.
    language: 'ar',
    dir: 'rtl',
    fontFamily: {
      supportAllValues: true
    },
    fontSize: {
      options: [9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26],
      supportAllValues: true
    }
  };

  hearingId: number = 0;
  displayedColumns: string[] = [
    'position',
    'reviewNumber',
    'createdOn',
    'createdBy',
    'noteText',
  ];

  constructor(
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private legalMemoService: LegalMemoService,
    private caseService: CaseService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    public location: Location,
    public authService: AuthService,
    private dialog: MatDialog

  ) {
    this.activatedRouter.paramMap.subscribe((params) => {

      var id = params.get('id');
      if (id != null) {
        this.id = +id;
      }
    });
  }

  ngOnInit() {
    this.init();
    this.populateLegalMemoStatus();
    this.populateLegalMemoTypes();
    // edit legal memo
    if (this.id) {
      this.subs.add(
        this.legalMemoService.get(this.id).subscribe(
          (result: any) => {

            this.patchForm(result.data);
            this.caseNumberInSource = result.data.initialCaseNumber;
            this.memoStatus = result.data.status.id;
            if (this.memoStatus == LegalMemoStatus.Returned) {
              this.populateLegalMemoNotes();
            }
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }

  }
  /**
     * Component init.
     */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
      status: [{ value: LegalMemoStatus.New, disabled: true }],
      type: [null, Validators.compose([Validators.required])],
      text: [null, Validators.compose([Validators.required])],
      isRead: [null],
      initialCaseId: [null, Validators.compose([Validators.required])],
      secondSubCategoryId: [null]
    });
  }

  patchForm(memo) {
    this.secondSubCategory = memo.secondSubCategory.name;
    this.form.patchValue({
      id: memo.id,
      name: memo.name,
      status: memo.status.id,
      type: memo.type.id,
      text: memo.text,
      isRead: memo.isRead,
      initialCaseId: memo.initialCaseId,
      secondSubCategoryId: [memo.secondSubCategory.id]
    });

    this.onCaseChanged(memo.initialCaseId);
  }

  populateLegalMemoStatus() {
    this.subs.add(
      this.legalMemoService.getLegalMemoStatus().subscribe(
        (result: any) => {
          this.legalMemoStatus = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateLegalMemoTypes() {
    this.subs.add(
      this.legalMemoService.getLegalMemoTypes().subscribe(
        (result: any) => {
          this.legalMemoTypes = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateLegalMemoNotes() {
    this.noteQueryObject.legalMemoId = this.id;
    this.subs.add(
      this.legalMemoService.getNotesWithQuery(this.noteQueryObject).subscribe(
        (result: any) => {
          this.dataSource = new MatTableDataSource(result.data.items);
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onChooseCase() {
    this.caseQueryObject.legalMemoType = this.form.value.type;
    const dialogRef = this.dialog.open(HearingSearchCasesComponent, {
      width: '90%',
      height: '90%',
      data: this.caseQueryObject
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res: CaseListItem) => {
          if (res) {
            this.onCaseChanged(res.id);
            this.caseNumberInSource= res.caseNumberInSource;
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  onChangeMemoType() {
    this.caseId = null;
    this.form.controls["initialCaseId"].setValue(null);
    this.form.controls["secondSubCategoryId"].setValue(null);
  }
  onCaseChanged(caseId: any) {
    this.caseId = caseId;
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.get(caseId).subscribe(
        (result: any) => {
          this.loaderService.stopLoading();
          if (result.data) {
            this.caseSubject = result.data?.subject;
            this.form.controls["initialCaseId"].setValue(caseId);
            this.form.controls["secondSubCategoryId"].setValue(result.data.secondSubCategory.id);
            this.mainCategory = result.data.subCategory.mainCategory.name;
            this.firstSubCategory = result.data.subCategory.firstSubCategory.name;
            this.secondSubCategory = result.data.secondSubCategory.name;
          }
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  isNameExist() {
    const nameCtrl = this.form.get('name');

    if (this.form.value.name) {
      this.memoQueryObject.name = this.form.value.name;
      this.subs.add(
        this.legalMemoService
          .getWithQuery(this.memoQueryObject)
          .subscribe((res: any) => {
            if (res.data.items && res.data.items.length > 0)
              if (res.data.items[0].id != this.form.value.id)
                nameCtrl?.setErrors({ nameExists: true });
          })
      );
    }
  }

  raiseConsultant() {
    this.form.patchValue({
      status: LegalMemoStatus.RaisingConsultant,
    });
    this.onSubmit();
  }

  onSubmit() {
    let result$: any;
    this.form.controls['status'].enable();

    // edit legal memo
    if (this.id) {
      // check if the form data has changed
      if (!this.form.dirty) {
        // data NOT changed and memo new or returned
        if (
          this.form.value.status == LegalMemoStatus.New ||
          this.form.value.status == LegalMemoStatus.Returned
        ) {
          this.alert.error(
            'لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!'
          );
          return;
        }
      }
      result$ = this.legalMemoService.update(this.form.value);
    }
    // new legal memo
    else {
      result$ = this.legalMemoService.create(this.form.value);
    }

    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      result$.subscribe(
        () => {
          this.loaderService.stopLoading();
          let message = this.id
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
          this.router.navigate(['/memos/list']);
        },
        (error: any) => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = this.id
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);

          this.form
            .get('updatedOn')
            ?.setValue(
              this.hijriConverter.gregorianToHijri(
                this.form.get('updatedOn')?.value
              )
            );
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
