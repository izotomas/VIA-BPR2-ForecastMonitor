import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InstallationComponent } from './installation.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MaterialModule } from '../../../material/material.module';
import { ClientComponent } from '../client/client.component';
import { ModelComponent } from '../model/model.component';
import { PerformanceIndicatorComponent } from '../../performance-indicator/performance-indicator.component';
import { TruncatePipe } from '../../../pipes/truncate.pipe';
import { ErrorHandler } from '../../../services/error-handler.service';

describe('InstallationComponent', () => {
  let component: InstallationComponent;
  let fixture: ComponentFixture<InstallationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule, MaterialModule],
      declarations: [
        InstallationComponent,
        ClientComponent,
        ModelComponent,
        PerformanceIndicatorComponent,
        TruncatePipe
      ],
      providers: [ErrorHandler]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InstallationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
