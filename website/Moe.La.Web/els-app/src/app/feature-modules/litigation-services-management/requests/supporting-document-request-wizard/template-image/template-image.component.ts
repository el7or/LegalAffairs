import { Component, OnInit } from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Inject } from '@angular/core';

@Component({
  selector: 'app-template-image',
  templateUrl: './template-image.component.html',
  styleUrls: ['./template-image.component.css']
})
export class TemplateImageComponent implements OnInit {

  name: string;
  thumbnail :string;

  constructor(public dialogRef: MatDialogRef<TemplateImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any){ }

  ngOnInit(): void {
    this.name = this.data.name;
    this.thumbnail = this.data.image;
  }
  closeDialog() {
    this.dialogRef.close();
  }
}
