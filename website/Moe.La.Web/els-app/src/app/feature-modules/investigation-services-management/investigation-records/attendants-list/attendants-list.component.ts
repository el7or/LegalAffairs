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

import { InvestigationRecordAttendant } from 'app/core/models/investigation-record';
import { AttendantFormComponent } from '../attendants-form/attendants-form.component';

@Component({
  selector: 'app-attendants-list',
  templateUrl: './attendants-list.component.html',
  styleUrls: ['./attendants-list.component.css'],
})
export class AttendantsListComponent implements OnInit, OnChanges, OnDestroy {

  @Input() readOnly: boolean = false;

  @Input('attendantsToUpdate') attendantsToUpdate: InvestigationRecordAttendant[] = [];

  @Output('set-attendants-list') setAttendantsList = new EventEmitter<InvestigationRecordAttendant[]>();

  displayedColumns: string[] = [
    'fullName',
    'assignedWork',
    'workLocation',
    'representativeOf',
    'details',
    'actions',
  ];

  attendantsDataSource!: MatTableDataSource<InvestigationRecordAttendant>;
  attendantsList: InvestigationRecordAttendant[] = [];

  @ViewChild('fileInput', { static: true }) fileInput: ElementRef = null!;

  private subs = new Subscription();

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['attendantsToUpdate']) {
      this.attendantsList = changes['attendantsToUpdate'].currentValue;
      this.attendantsDataSource = new MatTableDataSource(this.attendantsList);
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }


  onDelete(index: any) {
    this.attendantsList.splice(index, 1);
    this.attendantsDataSource = new MatTableDataSource(this.attendantsList);
    this.setAttendantsList.emit(this.attendantsList);
  }

  onAdd() {
    this.subs.add(
      this.dialog.open(AttendantFormComponent, {
        width: '40em',
      }).afterClosed().subscribe(
        (res: InvestigationRecordAttendant) => {
          if (res) {
            this.attendantsList.push(res);
            this.attendantsDataSource = new MatTableDataSource(this.attendantsList);
            // send it to component
            this.setAttendantsList.emit(this.attendantsList);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );

  }

  onEdit(attendant: any, index: number) {
    this.subs.add(
      this.dialog.open(AttendantFormComponent, {
        width: '40em',
        data: { attendant }
      }).afterClosed().subscribe(
        (res: InvestigationRecordAttendant) => {
          if (res) {
            this.attendantsList[index] = res;
            this.attendantsDataSource = new MatTableDataSource(this.attendantsList);
            // send it to component
            this.setAttendantsList.emit(this.attendantsList);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
}
