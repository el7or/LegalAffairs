import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ExportCaseJudgmentRequestReplyFormComponent } from './export-case-judgment-request-reply-form.component';

describe('ExportCaseJudgmentRequestReplyFormComponent', () => {
  let component: ExportCaseJudgmentRequestReplyFormComponent;
  let fixture: ComponentFixture<ExportCaseJudgmentRequestReplyFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportCaseJudgmentRequestReplyFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportCaseJudgmentRequestReplyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
