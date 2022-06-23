import { Component } from '@angular/core';

@Component({
  selector: 'app-loader',
  template:
    `<mat-card>
        <mat-card-header>
            <mat-card-title>
                جاري التحميل...
            </mat-card-title>
        </mat-card-header>
        <mat-card-content>
            <mat-progress-spinner mode="indeterminate"></mat-progress-spinner>
        </mat-card-content>
    </mat-card>`
})
export class LoaderComponent {

  constructor() { }
}
