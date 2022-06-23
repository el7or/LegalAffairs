import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InvestigationRecordListComponent } from './investigation-record-list.component';

describe('InvestigationRecordListComponent', () => {
  let component: InvestigationRecordListComponent;
  let fixture: ComponentFixture<InvestigationRecordListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InvestigationRecordListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestigationRecordListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
