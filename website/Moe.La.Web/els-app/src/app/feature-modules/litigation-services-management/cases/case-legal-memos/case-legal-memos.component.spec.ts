import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CaseLegalMemosComponent } from './case-legal-memos.component';

describe('CaseLegalMemosComponent', () => {
  let component: CaseLegalMemosComponent;
  let fixture: ComponentFixture<CaseLegalMemosComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CaseLegalMemosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CaseLegalMemosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
