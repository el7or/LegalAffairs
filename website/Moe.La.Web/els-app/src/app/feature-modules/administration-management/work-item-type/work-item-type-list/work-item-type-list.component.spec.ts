import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WorkItemTypeListComponent } from './work-item-type-list.component';

describe('WorkItemTypeListComponent', () => {
  let component: WorkItemTypeListComponent;
  let fixture: ComponentFixture<WorkItemTypeListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkItemTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkItemTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
