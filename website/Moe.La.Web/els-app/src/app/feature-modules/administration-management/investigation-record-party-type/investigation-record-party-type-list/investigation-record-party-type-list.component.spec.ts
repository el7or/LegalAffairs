import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InvestigationRecordPartyTypeListComponent } from './investigation-record-party-type-list.component';

describe('InvestigationRecordPartyTypeListComponent', () => {
  let component: InvestigationRecordPartyTypeListComponent;
  let fixture: ComponentFixture<InvestigationRecordPartyTypeListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InvestigationRecordPartyTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestigationRecordPartyTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
