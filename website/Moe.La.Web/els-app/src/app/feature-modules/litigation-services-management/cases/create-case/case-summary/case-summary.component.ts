import {
  Component,
  Input, 
  SimpleChanges,
  OnInit
} from '@angular/core';

import { MatTableDataSource } from '@angular/material/table';
import { PartyTypes } from 'app/core/enums/PartyTypes';
import { CaseService } from 'app/core/services/case.service';
import { Subscription } from 'rxjs';
import { CasePartyDetails } from 'app/core/models/case-party';
import { MainCaseDetails} from 'app/core/models/case'; 
import { CaseGround } from 'app/core/models/case-ground';
import { GroupNames } from 'app/core/models/attachment';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';

@Component({
  selector: 'app-case-summary',
  templateUrl: './case-summary.component.html',
  styleUrls: ['./case-summary.component.css']
})
export class CaseSummaryComponent implements OnInit {

  @Input() caseId: number;
  @Input() case: MainCaseDetails;

  private subs = new Subscription();
  caseParties: CasePartyDetails[];
  caseGrounds: CaseGround[];
  partiesDataSource!: MatTableDataSource<CasePartyDetails>;
  groundsDataSource!: MatTableDataSource<CaseGround>;
  hearings!: MatTableDataSource<any>;

  displayedColumns: string[] = [
    'name',
    'partyType'
  ];
  caseGroundsDisplayedColumns: string[] = ['text'];

  public get PartyTypes(): typeof PartyTypes {
    return PartyTypes;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }
  basicPanelOpenState = true;
  partyPanelOpenState = false;
  groundsPanelOpenState = false;
  hearingsPanelOpenState = false;
  attachmentPanelOpenState = false;

  constructor(private caseService: CaseService) { }

  ngOnInit(): void {
    this.populateCase();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      if(this.case != null)
      {
        this.caseParties = this.case.caseParties;
        this.caseGrounds = this.case.caseGrounds;
        this.partiesDataSource = new MatTableDataSource(this.caseParties);
        this.groundsDataSource = new MatTableDataSource(this.caseGrounds);
        this.hearings = new MatTableDataSource(this.case.hearings);
      }
    }
  }

  populateCase() {
        this.subs.add(
          this.caseService.get(this.caseId).subscribe(
            (result: any) => {
              this.case = result.data;
              this.caseParties = this.case.caseParties;
              this.caseGrounds = this.case.caseGrounds;
              this.partiesDataSource = new MatTableDataSource(this.caseParties);
              this.groundsDataSource = new MatTableDataSource(this.caseGrounds);
            },
            (error) => {
              console.error(error);
            }
          )
        );
  }
}
