import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WorkItemTypeFormComponent } from './work-item-type-form.component';

describe('WorkItemTypeFormComponent', () => {
  let component: WorkItemTypeFormComponent;
  let fixture: ComponentFixture<WorkItemTypeFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkItemTypeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkItemTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
