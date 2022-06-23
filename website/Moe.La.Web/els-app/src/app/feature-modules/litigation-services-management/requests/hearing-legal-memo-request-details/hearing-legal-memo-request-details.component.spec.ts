import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { HearingLegalMemoRequestDetailsComponent } from './hearing-legal-memo-request-details.component';

describe('HearingLegalMemoRequestDetailsComponent', () => {
  let component: HearingLegalMemoRequestDetailsComponent;
  let fixture: ComponentFixture<HearingLegalMemoRequestDetailsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ HearingLegalMemoRequestDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HearingLegalMemoRequestDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
