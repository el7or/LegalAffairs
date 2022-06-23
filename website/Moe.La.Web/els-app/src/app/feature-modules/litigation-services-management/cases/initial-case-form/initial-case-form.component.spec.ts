import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InitialCaseFormComponent } from './initial-case-form.component';

describe('InitialCaseFormComponent', () => {
  let component: InitialCaseFormComponent;
  let fixture: ComponentFixture<InitialCaseFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InitialCaseFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InitialCaseFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
