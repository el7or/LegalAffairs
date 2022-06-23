import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { HearingLegalMemoRequestReplyFormComponent } from './hearing-legal-memo-request-reply-form.component';

describe('HearingLegalMemoRequestReplyFormComponent', () => {
  let component: HearingLegalMemoRequestReplyFormComponent;
  let fixture: ComponentFixture<HearingLegalMemoRequestReplyFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ HearingLegalMemoRequestReplyFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HearingLegalMemoRequestReplyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
