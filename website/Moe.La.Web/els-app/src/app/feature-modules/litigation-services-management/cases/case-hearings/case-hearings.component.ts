import { Component, Input, OnInit } from '@angular/core';

import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-case-hearings',
  templateUrl: './case-hearings.component.html',
  styleUrls: ['./case-hearings.component.css'],
})
export class CaseHearingsComponent implements OnInit {
  @Input('hearings') hearings!: any;
  @Input('case') case!: any;
  displayedColumns: string[] = [
    'hearingDate',
    'hearingTime',
    'type',
    'status',
    'court',
    'circleNumber',
    //'hearingDetails',
  ];
  isAnyHearingTypeJudgment: boolean = false;

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  isResearcher: boolean = this.authService.checkRole(
    AppRole.LegalResearcher, Department.Litigation
  );

  constructor(private authService: AuthService) { }

  ngOnInit() {
    if (this.case) {
      this.isAnyHearingTypeJudgment = this.hearings?.data.some(h => h.type == "نطق بالحكم");
    }
  }
}
