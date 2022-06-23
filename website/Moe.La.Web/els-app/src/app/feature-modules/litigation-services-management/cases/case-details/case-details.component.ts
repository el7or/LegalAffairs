import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { CaseDetails } from 'app/core/models/case';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AppRole } from 'app/core/models/role';
import { AuthService } from 'app/core/services/auth.service';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { CaseSources } from 'app/core/enums/CaseSources';
import { Department } from 'app/core/enums/Department';
import { GroupNames } from 'app/core/models/attachment';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';

@Component({
  selector: 'app-case-details',
  templateUrl: './case-details.component.html',
  styleUrls: ['./case-details.component.css']
})
export class CaseDetailsComponent implements OnInit, OnDestroy {
  caseId: number = 0;
  @Input('case') case!: CaseDetails;
  partiesDisplayedColumns: string[] = [
    'position',
    'name',
    'partyType',
  ];
  transactionsDisplayedColumns: string[] = [
    'position',
    'createdOn',
    'createdOnTime',
    'createdBy',
    'TransactionType',
    'transactionText'];

  private subs = new Subscription();

  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }
  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get CaseSources(): typeof CaseSources {
    return CaseSources;
  }

  
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);

  constructor(
    private caseService: CaseService,
    private loaderService: LoaderService,
    private alert: AlertService,
    public authService: AuthService,

  ) { }

  ngOnInit(): void {
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.printCaseDetails(this.case).subscribe(
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

  isCaseResearcher(researchers: any[]) {
    if (researchers)
      return researchers.find((m) => m.id == this.authService.currentUser?.id);
  }
}
