import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PartiesDetailsComponent } from './parties-details.component';

describe('PartiesDetailsComponent', () => {
  let component: PartiesDetailsComponent;
  let fixture: ComponentFixture<PartiesDetailsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PartiesDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartiesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
