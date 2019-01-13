import { TestBed } from '@angular/core/testing';

import { TranslationService } from './translation.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ErrorHandler } from './error-handler.service';
import { MaterialModule } from '../material/material.module';

describe('TranslationService', () => {
  let service: TranslationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MaterialModule],
      providers: [TranslationService, ErrorHandler]
    });
    service = TestBed.get(TranslationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have fetchTranslations method', () => {
    expect(service.fetchTranslations).toBeDefined();
  });
});
