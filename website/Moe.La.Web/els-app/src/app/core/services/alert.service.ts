import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private snackBar: MatSnackBar) { }

  error(message:string):void{
    this.snackBar.open(message, ' ', {
      duration: 5000,
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
      panelClass: ['bg-danger'],
    });
  }

  succuss(message:string):void{
    this.snackBar.open(message, ' ', {
      duration: 5000,
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
      panelClass: ['bg-success'],
    });
  }

  info(message:string):void{
    this.snackBar.open(message, ' ', {
      duration: 5000,
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
      panelClass: ['bg-info'],
    });
  }
}
