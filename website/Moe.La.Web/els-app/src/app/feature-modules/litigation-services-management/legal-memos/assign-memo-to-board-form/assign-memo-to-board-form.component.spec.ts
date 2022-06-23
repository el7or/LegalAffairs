import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { AssignMemoToBoardFormComponent } from './assign-memo-to-board-form.component';

describe('AssignMemoToBoardFormComponent', () => {
  let component: AssignMemoToBoardFormComponent;
  let fixture: ComponentFixture<AssignMemoToBoardFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignMemoToBoardFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignMemoToBoardFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
