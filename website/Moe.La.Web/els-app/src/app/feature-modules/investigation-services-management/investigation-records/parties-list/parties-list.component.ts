import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { InvestigationRecordParty, InvestigationRecordPartyDetails } from 'app/core/models/investigation-record';
import { PartiesFormComponent } from '../parties-form/parties-form.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { PartiesDetailsComponent } from '../parties-details/parties-details.component';

@Component({
  selector: 'app-parties-list',
  templateUrl: './parties-list.component.html',
  styleUrls: ['./parties-list.component.css'],
})
export class PartiesListComponent implements OnInit, OnChanges, OnDestroy {

  @Input() readOnly: boolean = false;

  @Input('partiesToUpdate') partiesToUpdate: InvestigationRecordPartyDetails[] = [];

  @Output('set-parties-list') setParties = new EventEmitter<InvestigationRecordParty[]>();

  displayedColumns: string[] = [
    'fullName',
    'birthDate',
    'assignedWork',
    'workLocation',
    'staffType',
    'partyType',
    'actions',
  ];

  partiesDataSource!: MatTableDataSource<InvestigationRecordParty>;
  parties: InvestigationRecordParty[] = [];

  isInvestigatorManager = this.authService.checkRole(AppRole.DepartmentManager, Department.Investiation);//AppRole.InvestigationManager
  @ViewChild('fileInput', { static: true }) fileInput: ElementRef = null!;

  private subs = new Subscription();

  constructor(private dialog: MatDialog,
    private authService: AuthService,) { }

  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['partiesToUpdate']) {
      this.parties = changes['partiesToUpdate'].currentValue;
      this.partiesDataSource = new MatTableDataSource(this.parties);
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }


  onDelete(index: any) {
    this.parties.splice(index, 1);
    this.partiesDataSource = new MatTableDataSource(this.parties);
    this.setParties.emit(this.parties);
  }

  onAdd() {
    this.subs.add(
      this.dialog.open(PartiesFormComponent, {
        width: '40em',
      }).afterClosed().subscribe(
        (res: any) => {
          if (res) {

            this.parties.push(res);

            this.partiesDataSource = new MatTableDataSource(this.parties);
            // send it to component
            this.setParties.emit(this.parties);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );

  }

  onEdit(party: any, index: number) {
    this.subs.add(
      this.dialog.open(PartiesFormComponent, {
        width: '40em',
        data: { party }
      }).afterClosed().subscribe(
        (res: InvestigationRecordParty) => {
          if (res) {
            this.parties[index] = res;
            this.partiesDataSource = new MatTableDataSource(this.parties);
            // send it to component
            this.setParties.emit(this.parties);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
  onView(party: any, index: number) {
    this.dialog.open(PartiesDetailsComponent, {
      width: '40em',
      data: { party }
    });
  }
}
