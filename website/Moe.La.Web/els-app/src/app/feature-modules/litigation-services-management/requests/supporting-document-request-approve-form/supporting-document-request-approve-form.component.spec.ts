import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CaseSupportingDocumentRequestApproveFormComponent } from './supporting-document-request-approve-form.component';

describe('CaseSupportingDocumentRequestApproveFormComponent', () => {
  let component: CaseSupportingDocumentRequestApproveFormComponent;
  let fixture: ComponentFixture<CaseSupportingDocumentRequestApproveFormComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CaseSupportingDocumentRequestApproveFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CaseSupportingDocumentRequestApproveFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
