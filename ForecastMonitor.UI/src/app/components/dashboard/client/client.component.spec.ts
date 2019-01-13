import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientComponent } from './client.component';
import { PerformanceIndicatorComponent } from '../../performance-indicator/performance-indicator.component';
import { ModelComponent } from '../model/model.component';
import { MaterialModule } from '../../../material/material.module';
import { RouterTestingModule } from '@angular/router/testing';
import { TruncatePipe } from '../../../pipes/truncate.pipe';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ErrorHandler } from '../../../services/error-handler.service';

describe('ClientComponent', () => {
  let component: ClientComponent;
  let fixture: ComponentFixture<ClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule, MaterialModule],
      declarations: [
        ClientComponent,
        ModelComponent,
        PerformanceIndicatorComponent,
        TruncatePipe
      ],
      providers: [ErrorHandler]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
