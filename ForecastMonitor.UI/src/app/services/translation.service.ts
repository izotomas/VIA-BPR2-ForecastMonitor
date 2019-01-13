import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ErrorHandler, Handler } from './error-handler.service';
import { catchError } from 'rxjs/operators';

interface TranslationsResponse {
  navigation?: {
    brand: string;
  };
  dashboard?: {
    pageHeader: string;
  };
  model?: {
    label: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private handler: Handler;
  constructor(private http: HttpClient, private errorHandler: ErrorHandler) {
    this.handler = this.errorHandler.createErrorHandler('TranslationService');
  }

  /**
   * Fetch translations file
   */
  fetchTranslations = () =>
    this.http
      .get<TranslationsResponse>('assets/translations.json')
      .pipe(catchError(this.handler('Get Installations').handleError))
}
