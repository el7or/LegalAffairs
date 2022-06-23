import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Component, ViewChild, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { AuthService } from 'app/core/services/auth.service';
import { MoamalaChangeStatus, MoamalaDetails, MoamalatListItem } from 'app/core/models/moamalat';
import { MoamalatQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { MoamalaStatuses, MoamalaSteps } from 'app/core/enums/MoamalaStatuses';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { ChangeMoamalaStatusFormComponent } from '../../change-moamala-status-form/change-moamala-status-form.component';
import { GroupNames } from 'app/core/models/attachment';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-moamalat-list-table',
  templateUrl: './moamalat-list-table.component.html',
  styleUrls: ['./moamalat-list-table.component.css'],
})
export class MoamalatListTableComponent implements OnChanges {
  @Input('dataSource') dataSource!: MatTableDataSource<MoamalatListItem>;
  @Input('hideActions') hideActions: boolean = false;

  displayedColumns: string[] = [
    'unifiedNo',
    'moamalaNumber',
    'createdOn',
    'passDate',
    'subject',
    //'description',
    'confidentialDegree',
    'status',
    'senderDepartment',
    'branch',
    'createdBy',
    'actions',
  ];

  totalItems!: number;
  queryObject: MoamalatQueryObject = new MoamalatQueryObject();

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);

  @ViewChild(MatSort) sort!: MatSort;

  private subs = new Subscription();


  public get MoamalaStatuses(): typeof MoamalaStatuses {
    return MoamalaStatuses;
  }
  public get ConfidentialDegrees(): typeof ConfidentialDegrees {
    return ConfidentialDegrees;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public get MoamalaSteps(): typeof MoamalaSteps {
    return MoamalaSteps;
  }
  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  @Output('set-data-list') populateMoamalat = new EventEmitter<any>(); // value will returns to $event variable

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
    private loaderService: LoaderService,
    public authService: AuthService,
    private alert: AlertService,
    private dialog: MatDialog
  ) { }

  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.populateMoamalat.emit({ page: null, sortBy: result.active, isSortAscending: result.direction == 'asc' ? 'true' : 'false' });
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['dataSource']) {
      this.dataSource = changes['dataSource'].currentValue;
    }
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
            this.populateMoamalat.emit();

          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }


  onPageChange(page: number) {
    this.populateMoamalat.emit({ page: page + 1 })
  }

  onDelete(moamalaId: number) {
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
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.moamalatService.delete(moamalaId).subscribe(
            () => {
              this.dataSource.data.forEach((item, index) => {
                if (item.id == moamalaId) {
                  this.dataSource.data.splice(index, 1);
                  this.dataSource = new MatTableDataSource(this.dataSource.data);
                }
              });
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
            },
            (error: any) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

}
