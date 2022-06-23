import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-case-legal-memos',
  templateUrl: './case-legal-memos.component.html',
  styleUrls: ['./case-legal-memos.component.css']
})
export class CaseLegalMemosComponent implements OnInit {
  @Input('legalMemos') legalMemos: any | undefined;
  constructor() { }

  ngOnInit(): void {
  }

}
