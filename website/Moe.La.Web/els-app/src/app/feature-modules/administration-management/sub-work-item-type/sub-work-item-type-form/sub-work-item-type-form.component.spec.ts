import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SubWorkItemTypeFormComponent } from './sub-work-item-type-form.component';

describe('SubWorkItemTypeFormComponent', () => {
  let component: SubWorkItemTypeFormComponent;
  let fixture: ComponentFixture<SubWorkItemTypeFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SubWorkItemTypeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubWorkItemTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
