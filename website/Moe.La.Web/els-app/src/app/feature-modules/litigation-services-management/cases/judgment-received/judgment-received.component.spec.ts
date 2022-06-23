import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JudgmentReceivedComponent } from './judgment-received.component';

describe('JudgmentReceivedComponent', () => {
  let component: JudgmentReceivedComponent;
  let fixture: ComponentFixture<JudgmentReceivedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JudgmentReceivedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JudgmentReceivedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
