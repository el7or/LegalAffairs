import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { AddManualPartyFormComponent } from '../add-manual-party-form/add-manual-party-form.component';
import { PartyTypes } from 'app/core/enums/PartyTypes';
import { CaseParty } from 'app/core/models/case-party';

@Component({
  selector: 'app-manual-parties-list',
  templateUrl: './manual-parties-list.component.html',
  styleUrls: ['./manual-parties-list.component.css']
})
export class ManualPartiesListComponent implements OnChanges, OnDestroy {

  @Input() readOnly: boolean = false;

  @Input('partiesToUpdate') partiesToUpdate: CaseParty[] = [];

  @Output('set-parties-list') setParties = new EventEmitter<CaseParty[]>();

  displayedColumns: string[] = [
    'name',
    'partyType',
    'actions',
  ];


  partiesDataSource!: MatTableDataSource<CaseParty>;
  parties: CaseParty[] = [];
  private subs = new Subscription();

  public get PartyTypes(): typeof PartyTypes {
    return PartyTypes;
  }
  constructor(private dialog: MatDialog) { }



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
      this.dialog.open(AddManualPartyFormComponent, {
        width: '40em',
        data: { parties: this.parties }

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
    let partiesToCompare = [];
    Object.assign(partiesToCompare, this.parties);

    partiesToCompare.splice(index, 1);

    this.subs.add(
      this.dialog.open(AddManualPartyFormComponent, {
        width: '40em',
        data: { party, parties: partiesToCompare }
      }).afterClosed().subscribe(
        (res: CaseParty) => {
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
}
