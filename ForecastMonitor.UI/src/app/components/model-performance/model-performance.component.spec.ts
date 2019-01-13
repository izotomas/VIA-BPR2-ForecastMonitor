import {
  async,
  ComponentFixture,
  TestBed,
  tick,
  fakeAsync
} from '@angular/core/testing';

import { ModelPerformanceComponent } from './model-performance.component';
import { MaterialModule } from '../../material/material.module';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ErrorHandler } from '../../services/error-handler.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { APP_BASE_HREF } from '@angular/common';
import { DataService } from 'src/app/services/data.service';

import { environment } from '../../../environments/environment';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'angular2-toaster';

class ActivatedRouteStub {
  params = of({});

  set testParams(params: any) {
    this.params = of(params);
  }
}

describe('ModelPerformanceComponent', () => {
  let component: ModelPerformanceComponent;
  let fixture: ComponentFixture<ModelPerformanceComponent>;

  let service: DataService;
  let activatedRoute: ActivatedRouteStub;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MaterialModule,
        RouterTestingModule,
        HttpClientTestingModule,
        NgxChartsModule
      ],
      declarations: [ModelPerformanceComponent],
      providers: [
        ErrorHandler,
        ToasterService,
        DataService,
        {
          provide: APP_BASE_HREF,
          useValue: '/'
        },
        {
          provide: ActivatedRoute,
          useValue: new ActivatedRouteStub()
        }
      ]
    }).compileComponents();

    service = TestBed.get(DataService);
    spyOn(service, 'getUnitPlot');
    activatedRoute = TestBed.get(ActivatedRoute);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModelPerformanceComponent);
    component = fixture.componentInstance;
    activatedRoute.testParams = { installation_id: 1, unit_id: 512 };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should configure the interval', () => {
    expect(component.interval).toBe(environment.defaultInterval);
    expect(component.maxInterval).toBe(environment.maxInterval);
    expect(component.minInterval).toBe(environment.minInterval);
  });

  it('should setup update source', () => {
    expect(component['updater']).toBeDefined();
    expect(component.onIntervalChange).toBeDefined();
  });

  it('should setup installationId and unitId', () => {
    expect(component['installation_id']).toBeDefined();
    expect(component['unit_id']).toBeDefined();
  });

  it('should request data for 2 weeks', () => {
    const installation_id = component['installation_id'];
    const unit_id = component['unit_id'];

    expect(service.getUnitPlot).toHaveBeenCalledTimes(1);
    expect(service.getUnitPlot).toHaveBeenCalledWith(
      installation_id,
      unit_id,
      2
    );
  });
});
