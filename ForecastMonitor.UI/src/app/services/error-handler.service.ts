import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { tap, scan, delay } from 'rxjs/operators';

import { MatDialog, MatDialogRef } from '@angular/material';
import { ErrorComponent } from '../modals/error/error.component';

import { environment } from '../../environments/environment';

/** Types for the hanler functions */
type HandleError = (error: HttpErrorResponse) => Observable<any>;
type RetryStrategy = (errors: Observable<any>) => Observable<any>;

/** Type of the handler returned by createErrorHandler */
export type Handler = (
  opeation?: string,
  result?: any,
  attempt?: number
) => {
  handleError: HandleError;
  retryStrategy: RetryStrategy;
};

@Injectable()
export class ErrorHandler {
  private dialogRef: MatDialogRef<ErrorComponent>;

  constructor(private dialog: MatDialog) {}

  /**
   * Create curried error handler that already knows the service name
   * @param serviceName - name of the data service that attempted the operation
   * @returns handler - instance of the handler
   * */
  createErrorHandler = (serviceName = '') => (
    operation = 'operation',
    result = {},
    attempt
  ) => ({
    handleError: this.handleError(serviceName, operation, result, attempt),
    retryStrategy: this.retryStrategy(serviceName, operation, result)
  })

  /**
   * Returns a function that handles Http operation failures.
   * This error handler lets the app continue to run as if no error occurred.
   * @param serviceName - name of the data service that attempted the operation
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   * @param attempt - the number of retry attempts
   */
  handleError = (
    serviceName: string,
    operation: string,
    result: any,
    attempt: number
  ) => (error: HttpErrorResponse) => {
    // TODO: send the error to remote logging infrastructure
    const message =
      error.error instanceof ErrorEvent
        ? error.error.message
        : `Backend returned code ${error.status}\n ` +
          `Message was: ${error.message}`;
    console.error(`${serviceName} -> ${operation}:\n ${message}\n`); // log to console instead

    if (this.dialogRef && this.dialogRef.componentInstance) {
      this.dialogRef.componentInstance.attempt = attempt;
    } else {
      // Open the error modal
      this.dialogRef = this.dialog.open(ErrorComponent, {
        panelClass: 'error-modal',
        data: { operation, attempt }
      });
    }

    // Let the app keep running by returning a safe result.
    return of(result);
  }

  /**
   * Returns a function that retries on Http operation failures
   * This retry strategy used the environment to retry http request and provide feedback
   * @param serviceName - name of the data service that attempted the operation
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  retryStrategy = (serviceName: string, operation: string, result: any) => (
    errors: Observable<any>
  ) => {
    let error;
    return errors.pipe(
      tap(e => (error = e)),
      scan(retryCount => {
        retryCount += 1;
        if (
          retryCount <= environment.retryAttempts ||
          environment.retryAttempts < 0
        ) {
          this.handleError(serviceName, operation, result, retryCount)(error);
          return retryCount;
        } else {
          throw new Error('Server is unreachable');
        }
      }, 0),
      delay(environment.retryDelay)
    );
  }
}
