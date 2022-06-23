import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import Swal from 'sweetalert2';

import { ConsultationStatus } from 'app/core/enums/ConsultationStatus';
import { MoamalaDetails, MoamalatListItem } from 'app/core/models/moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ConsultationService } from 'app/core/services/consultation.service';
import { ConsultationDetails } from 'app/core/models/consultation';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { SubWorkItemTypeService } from 'app/core/services/sub-work-item-type.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { ConsultationVisual } from 'app/core/models/consultation';
import { ConsultationSupportingDocument } from 'app/core/models/consultation-request';

@Component({
  selector: 'app-consultation-form-regulations-and-laws',
  templateUrl: './consultation-form-regulations-and-laws.component.html',
  styleUrls: ['./consultation-form-regulations-and-laws.component.css']
})

export class ConsultationFormRegulationsAndLawsComponent implements OnInit, OnDestroy {
  moamalaId: number = 0;
  consultationId: number = 0;
  consultationDetails!: ConsultationDetails;
  consultationVisuals: ConsultationVisual[] = [];
  moamala!: MoamalaDetails;
  relatedMoamalatDataSource!: MatTableDataSource<MoamalatListItem>;
  consultationVisualsDataSource: MatTableDataSource<ConsultationVisual>
  workItemTypes?: any[];
  subWorkItemTypes?: any[];
  consultationSupportingDocument: ConsultationSupportingDocument = new ConsultationSupportingDocument();

  form: FormGroup = Object.create(null);
  formConsultationVisuals: FormGroup = Object.create(null);

  visualsDisplayedColumns: string[] = [
    'position',
    'material',
    'visuals',
    'actions',
  ];

  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }

  private subs = new Subscription();

  constructor(
    private consultationService: ConsultationService,
    private moamalatService: MoamalatService,
    public workItemTypeService: WorkItemTypeService,
    public subWorkItemTypeService: SubWorkItemTypeService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private hijriConverter: HijriConverterService,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    public location: Location) {
    this.activatedRoute.paramMap.subscribe((params) => {
      // if new consultation
      var moamalaId = params.get('moamalaId');
      if (moamalaId != null) {
        this.moamalaId = +moamalaId;
      }
      // if edit consultation
      var id = params.get('id');
      if (id != null) {
        this.consultationId = +id;
      }
    }
    );
  }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);
    this.initForm();
    this.populateWorkItemType();

    // if new consultation
    if (this.moamalaId) {
      this.getMoamalaDetails(this.moamalaId);
    }
    // if edit consultation
    if (this.consultationId) {
      this.subs.add(
        this.consultationService.get(this.consultationId).subscribe(
          (result: any) => {
            this.consultationDetails = result.data;
            this.populateSubWorkItemType(this.consultationDetails.workItemType.id);
            this.getMoamalaDetails(this.consultationDetails.moamalaId);
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

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initForm() {
    this.form = this.fb.group({
      id: [0],
      moamalaId: [this.moamalaId],
      subject: [null, Validators.compose([Validators.required])],
      consultationNumber: [null, Validators.compose([Validators.required])],
      consultationDate: [null, Validators.compose([Validators.required])],
      importantElements: [null, Validators.compose([Validators.required])],
      legalAnalysis: [null, Validators.compose([Validators.required])],
      status: [ConsultationStatus.Draft],
      isWithNote: [false],
      workItemTypeId: [""],
      subWorkItemTypeId: [""],
      consultationVisuals: [null],
      branchId: [null],
      departmentId: [null],
    });
  }
  initVisualsForm() {
    this.formConsultationVisuals = this.fb.group({
      material: [null, Validators.compose([Validators.required])],
      visuals: [null, Validators.compose([Validators.required])],
    });
  }

  getMoamalaDetails(moamalaId: number) {
    this.subs.add(
      this.moamalatService.get(moamalaId).subscribe(
        (result: any) => {
          this.moamala = result.data;
          this.patchForm();
          this.relatedMoamalatDataSource = new MatTableDataSource(result.data.relatedMoamalat);
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت  عملية جلب البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  patchForm() {
    // if new consultation
    if (this.moamalaId) {
      this.form.patchValue({
        branchId: this.moamala?.branch?.id,
        departmentId: this.moamala?.senderDepartment?.id,
        moamalaId: this.moamalaId,
        subject: this.moamala.subject,
        workItemTypeId: this.moamala.workItemType.id,
        subWorkItemTypeId: this.moamala.subWorkItemType?.id,
        consultationNumber: this.moamala.moamalaNumber,
        consultationDate: this.moamala.createdOn
      });
      
      if (!this.moamala.subWorkItemType) {
        this.populateSubWorkItemType(this.moamala.workItemType.id);
      }
    }

    // if edit consultation
    else {
      this.form.patchValue({
        id: this.consultationDetails.id,
        branchId: this.consultationDetails.branch.id,
        departmentId: this.consultationDetails.department.id,
        moamalaId: this.consultationDetails.moamalaId,
        status: this.consultationDetails.status.id,
        subject: this.consultationDetails.subject,
        legalAnalysis: this.consultationDetails.legalAnalysis,
        importantElements: this.consultationDetails.importantElements,
        workItemTypeId: this.consultationDetails.workItemType.id,
        subWorkItemTypeId: this.consultationDetails.subWorkItemType?.id,
        consultationNumber: this.consultationDetails.consultationNumber,
        consultationDate: this.consultationDetails.consultationDate,
      });
      if (this.consultationDetails.isWithNote) {
        this.form.patchValue({ isWithNote: this.consultationDetails.isWithNote });
        this.consultationVisuals.push(...this.consultationDetails.consultationVisuals);
        this.consultationVisualsDataSource = new MatTableDataSource(this.consultationDetails.consultationVisuals);
        this.initVisualsForm();
      }
      if (this.consultationDetails.consultationSupportingDocuments) {
        this.consultationSupportingDocument = this.consultationDetails.consultationSupportingDocuments[0];
      }
    }
  }

  populateWorkItemType() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.workItemTypeService.getWithQuery({
        sortBy: 'id',
        isSortAscending: false,
        page: 1,
        pageSize: 30,
      }).subscribe(
        (result: any) => {
          this.workItemTypes = result.data.items;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  populateSubWorkItemType(workItemTypeId) {
    const queryObject = {
      sortBy: 'id',
      isSortAscending: true,
      page: 1,
      pageSize: 999,
      workItemTypeId: workItemTypeId
    }
    this.subs.add(
      this.subWorkItemTypeService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.subWorkItemTypes = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  addConsultationVisuals(formConsultationVisuals: FormGroup) {
    const newConsultationVisual: ConsultationVisual = {
      material: formConsultationVisuals.value.material,
      visuals: formConsultationVisuals.value.visuals
    }
    this.consultationVisuals.push(newConsultationVisual);
    this.consultationVisualsDataSource = new MatTableDataSource(this.consultationVisuals);
    this.initVisualsForm();
  }

  deleteConsultationVisuals(item: ConsultationVisual) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.consultationVisuals.forEach((value, index) => {
          if (value == item) this.consultationVisuals.splice(index, 1);
        });
        this.consultationVisualsDataSource = new MatTableDataSource(this.consultationVisuals);
      }
    });
  }

  onSubmit(status?: ConsultationStatus) {
    this.loaderService.startLoading(LoaderComponent);
    // if save & send
    if (status) {
      switch (status) {
        case ConsultationStatus.Draft:
          this.form.value.status = ConsultationStatus.New;
          break;

        case ConsultationStatus.Returned:
          this.form.value.status = ConsultationStatus.Modified;
          break;
      }
    }

    this.form.value.consultationDate = this.hijriConverter.calendarDateToDate(
      this.form.get('consultationDate')?.value?.calendarStart);
    this.form.value.consultationVisuals = this.consultationVisuals;

    let result$ = this.moamalaId
      ? this.consultationService.create(this.form.value)
      : this.consultationService.update(this.form.value);
    this.subs.add(
      result$.subscribe(
        () => {
          this.loaderService.stopLoading();
          let message = this.moamalaId
            ? 'تمت عملية الإضافة بنجاح'
            : 'تمت عملية التعديل بنجاح';
          this.alert.succuss(message);
          this.location.back();
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = this.moamalaId
            ? 'فشلت عملية الإضافة !'
            : 'فشلت عملية التعديل !';
          this.alert.error(message);
        }
      )
    );
  }
}
