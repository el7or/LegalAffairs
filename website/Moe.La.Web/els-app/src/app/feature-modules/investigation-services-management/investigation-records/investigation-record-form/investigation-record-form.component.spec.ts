import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InvestigationRecordFormComponent } from './investigation-record-form.component';

describe('InvestigationRecordFormComponent', () => {
  let component: InvestigationRecordFormComponent;
  let fixture: ComponentFixture<InvestigationRecordFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InvestigationRecordFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestigationRecordFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
