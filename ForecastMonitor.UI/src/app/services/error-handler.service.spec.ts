import { TestBed } from '@angular/core/testing';

import { ErrorHandler } from './error-handler.service';
import { MaterialModule } from '../material/material.module';

describe('ErrorHandler', () => {
  let service: ErrorHandler;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MaterialModule],
      providers: [ErrorHandler]
    });
    service = TestBed.get(ErrorHandler);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
