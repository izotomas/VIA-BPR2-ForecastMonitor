import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';

import { NavigationComponent } from './components/navigation/navigation.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MaterialModule } from './material/material.module';
import { ModelComponent } from './components/dashboard/model/model.component';
import { ToasterModule, ToasterService } from 'angular2-toaster';
import { ErrorHandler } from './services/error-handler.service';
import { TruncatePipe } from './pipes/truncate.pipe';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { InstallationComponent } from './components/dashboard/installation/installation.component';
import { ClientComponent } from './components/dashboard/client/client.component';
import { PerformanceIndicatorComponent } from './components/performance-indicator/performance-indicator.component';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MaterialModule,
        ToasterModule,
        HttpClientTestingModule
      ],
      declarations: [
        AppComponent,
        NavigationComponent,
        DashboardComponent,
        ModelComponent,
        InstallationComponent,
        ClientComponent,
        PerformanceIndicatorComponent,
        TruncatePipe
      ],
      providers: [ToasterService, ErrorHandler]
    }).compileComponents();
  }));
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
});
