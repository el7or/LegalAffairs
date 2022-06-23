import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SubWorkItemTypeListComponent } from './sub-work-item-type-list.component';

describe('SubWorkItemTypeListComponent', () => {
  let component: SubWorkItemTypeListComponent;
  let fixture: ComponentFixture<SubWorkItemTypeListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SubWorkItemTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubWorkItemTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
