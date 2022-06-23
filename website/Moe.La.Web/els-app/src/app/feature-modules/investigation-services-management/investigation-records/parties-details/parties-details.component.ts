import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';

import { EducationalLevel, Evaluation, InvestigationRecordPartyDetails, InvestigationRecordPartyPenalty } from 'app/core/models/investigation-record';

@Component({
  selector: 'app-parties-details',
  templateUrl: './parties-details.component.html',
  styleUrls: ['./parties-details.component.css']
})
export class PartiesDetailsComponent implements OnInit {
  party: InvestigationRecordPartyDetails = new InvestigationRecordPartyDetails();
  evaluationDisplayedColumns: string[] = [
    'position',
    'percentage',
    'year'
  ];
  educationLevelDisplayedColumns: string[] = [
    'position',
    'educationLevel',
    'class',
    'classNumber',
    'residenceAddress'

  ];
  penalitiesDisplayedColumns: string[] = [
    'position',
    'penalty',
    'reason',
    'decisionNumber',
    'date',
  ];
  evaluationsDataSource!: MatTableDataSource<Evaluation>;
  educationLevelDataSource!: MatTableDataSource<EducationalLevel>;
  penalitiesDataSource!: MatTableDataSource<InvestigationRecordPartyPenalty>;
  constructor(
    public dialogRef: MatDialogRef<PartiesDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data?.party) {
      this.party = this.data?.party;
      this.evaluationsDataSource = new MatTableDataSource(this.party.evaluations);
      this.penalitiesDataSource = new MatTableDataSource(this.party.investigationPartyPenalties);
      this.educationLevelDataSource = new MatTableDataSource(this.party.educationalLevels);
    }
  }

  ngOnInit(): void {
  }
  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
