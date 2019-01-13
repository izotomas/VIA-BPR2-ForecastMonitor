import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardComponent } from './dashboard.component';
import { MaterialModule } from '../../material/material.module';
import { ModelComponent } from './model/model.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ErrorHandler } from '../../services/error-handler.service';
import { TruncatePipe } from '../../pipes/truncate.pipe';
import { ClientComponent } from './client/client.component';
import { InstallationComponent } from './installation/installation.component';
import { RouterTestingModule } from '@angular/router/testing';
import { PerformanceIndicatorComponent } from '../performance-indicator/performance-indicator.component';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [MaterialModule, RouterTestingModule, HttpClientTestingModule],
      declarations: [
        DashboardComponent,
        ModelComponent,
        ClientComponent,
        InstallationComponent,
        PerformanceIndicatorComponent,
        TruncatePipe
      ],
      providers: [ErrorHandler]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
