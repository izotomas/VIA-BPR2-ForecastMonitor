import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformanceIndicatorComponent } from './performance-indicator.component';

describe('PerformanceIndicatorComponent', () => {
  let component: PerformanceIndicatorComponent;
  let fixture: ComponentFixture<PerformanceIndicatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PerformanceIndicatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformanceIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
