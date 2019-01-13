import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModelComponent } from './model.component';
import { MaterialModule } from '../../../material/material.module';
import { TruncatePipe } from '../../../pipes/truncate.pipe';
import { IUnit } from '../../../interfaces/iunit';
import { UnitStatus } from '../../../enums/unit-status.enum';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ErrorHandler } from '../../../services/error-handler.service';
import { RouterTestingModule } from '@angular/router/testing';
import { PerformanceIndicatorComponent } from '../../performance-indicator/performance-indicator.component';

describe('ModelComponent', () => {
  let component: ModelComponent;
  let fixture: ComponentFixture<ModelComponent>;

  const unit: IUnit = {
    id: 1,
    name: 'Test unit',
    client_id: 1,
    mae: 1
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [MaterialModule, HttpClientTestingModule, RouterTestingModule],
      declarations: [
        ModelComponent,
        TruncatePipe,
        PerformanceIndicatorComponent
      ],
      providers: [ErrorHandler]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModelComponent);
    component = fixture.componentInstance;
    component['model'] = unit;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should populate the instance variables', () => {
    expect(component.id).toEqual(unit.id);
    expect(component.name).toEqual(unit.name);
    expect(component.client_id).toEqual(unit.client_id);
    expect(component.mae).toBeDefined();
  });
});
