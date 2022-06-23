import { Location } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { Subscription } from 'rxjs';

import { AuthService } from 'app/core/services/auth.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalaChangeStatus, MoamalaDetails, MoamalatListItem } from 'app/core/models/moamalat';
import { GroupNames } from 'app/core/models/attachment';
import { MoamalaStatuses, MoamalaSteps } from 'app/core/enums/MoamalaStatuses';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { MatTableDataSource } from '@angular/material/table';
import { ChangeMoamalaStatusFormComponent } from '../change-moamala-status-form/change-moamala-status-form.component';
import { SubWorkItemTypeService } from 'app/core/services/sub-work-item-type.service';
import { AppRole } from 'app/core/models/role';
import { ConsultationStatus } from 'app/core/enums/ConsultationStatus';
import { MoamalatService } from 'app/core/services/moamalat.service';

@Component({
  selector: 'app-moamalat-view',
  templateUrl: './moamalat-view.component.html',
  styleUrls: ['./moamalat-view.component.css'],
})
export class MoamalatViewComponent implements OnInit, OnDestroy {
  moamalaId!: number;
  moamala!: MoamalaDetails;
  relatedMoamalatDataSource!: MatTableDataSource<MoamalatListItem>;
  updatingWorkItemType: boolean = false;
  workItemTypes?: any[];
  subWorkItemTypes?: any[];
  formWorkItemType: FormGroup = Object.create(null);

  subs = new Subscription();

  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public get ConfidentialDegrees(): typeof ConfidentialDegrees {
    return ConfidentialDegrees;
  }
  public get MoamalaStatuses(): typeof MoamalaStatuses {
    return MoamalaStatuses;
  }
  public get MoamalaSteps(): typeof MoamalaSteps {
    return MoamalaSteps;
  }
  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  isGeneralSupervisor: boolean = this.authService.checkRole(
    AppRole.GeneralSupervisor
  );
  isDistributor: boolean = this.authService.checkRole(
    AppRole.Distributor
  );
  isDepartmentManager: boolean = this.authService.checkRole(
    AppRole.DepartmentManager
  );
  isBranchManager: boolean = this.authService.checkRole(
    AppRole.BranchManager
  );

  constructor(
    private moamalatService: MoamalatService,
    private activatedRoute: ActivatedRoute,
    private loaderService: LoaderService,
    private alert: AlertService,
    public authService: AuthService,
    private fb: FormBuilder,
    public workItemTypeService: WorkItemTypeService,
    public subWorkItemTypeService: SubWorkItemTypeService,
    private dialog: MatDialog,
    private router: Router,
    private location: Location
  ) {
    this.activatedRoute.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.moamalaId = +id;
      }
    });
  }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.get(this.moamalaId).subscribe(
        (result: any) => {
          this.moamala = result.data;
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
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  updateWorkItemType() {
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
          this.updatingWorkItemType = true;
          this.getSubWorkItemType(this.moamala.workItemType?.id);
          this.formWorkItemType = this.fb.group({
            workItemTypeId: [this.moamala.workItemType?.id, Validators.compose([Validators.required])],
            subWorkItemTypeId: [this.moamala.subWorkItemType?.id, Validators.compose([Validators.required])],
          });
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

  getSubWorkItemType(workItemTypeId) {
    let queryObject = {
      sortBy: 'id',
      isSortAscending: false,
      page: 1,
      pageSize: 999,
      workItemTypeId: workItemTypeId
    }
    this.subs.add(
      this.subWorkItemTypeService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.subWorkItemTypes = result.data.items;
          this.formWorkItemType.patchValue(
            {
              subWorkItemTypeId: this.moamala.workItemType.id != workItemTypeId ? this.subWorkItemTypes[0].id : this.moamala.subWorkItemType.id
            });

        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onChangeWorkItemType() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.updateWorkItemType(this.moamalaId, this.formWorkItemType.value.workItemTypeId, this.formWorkItemType.value.subWorkItemTypeId).subscribe(
        (result: any) => {
          this.updatingWorkItemType = false;
          this.moamala = result.data;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onChangeReleatedItem(event: MatSelectChange) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.updateRelatedId(this.moamalaId, event.value).subscribe(
        (result: any) => {
          this.moamala = result.data;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.printMoamalaDetails(this.moamala).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.target = '_blank';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل طباعة البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  changeStatus(moamala: MoamalatListItem, status: MoamalaStatuses, currentStep: MoamalaSteps = null) {
    const moamalaChangeStatusData: MoamalaChangeStatus = {
      moamalaId: moamala.id,
      status: status,
      branchId: moamala.branch?.id,
      departmentId: moamala.receiverDepartment?.id,
      workItemTypeId: moamala.workItemType?.id,
      subWorkItemTypeId: moamala.subWorkItemType?.id,
      confidentialDegree: moamala.confidentialDegree?.id,
      currentStep:currentStep
    }
    const dialogRef = this.dialog.open(ChangeMoamalaStatusFormComponent, {
      width: '40em',
      data: {
        moamalaChangeStatusData: moamalaChangeStatusData
      },
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.router.navigate(["moamalat"]);
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }


  onCreateConsultation(moamalaId: number, workItemTypeName: string) {
    if (workItemTypeName == 'نظام' || workItemTypeName == 'لائحة' || workItemTypeName == 'قرار') {
      this.moamala.consultationId ? this.router.navigate(['/consultation/edit-laws', this.moamala.consultationId]) : this.router.navigate(['/consultation/new-laws', moamalaId]);
    } else {
      this.moamala.consultationId ? this.router.navigate(['/consultation/edit', this.moamala.consultationId]) : this.router.navigate(['/consultation/new', moamalaId]);
    }
  }

  onBack(): void {
    this.location.back();
  }
}
