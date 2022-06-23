import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-case-moamalat',
  templateUrl: './case-moamalat.component.html',
  styleUrls: ['./case-moamalat.component.css'],
})
export class CasemoamalatComponent implements OnInit {
  @Input('moamalat') moamalat!: any;
  @Input('case') case!: any;

  displayedColumns: string[] = [
    'moamalaNumber',
    'createdOn',
    'subject',
    'moamalaDetails'
  ];

  constructor(private authService: AuthService) { }

  ngOnInit() {

  }
}
