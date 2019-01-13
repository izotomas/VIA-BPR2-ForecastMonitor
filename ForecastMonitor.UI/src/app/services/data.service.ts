import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Error handling
import { catchError, retryWhen, tap } from 'rxjs/operators';
import { ErrorHandler, Handler } from '../services/error-handler.service';

import { environment } from '../../environments/environment';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

// Import Interfaces
import { IUnit } from '../interfaces/iunit';
import { IClient } from '../interfaces/iclient';
import { IInstallation } from '../interfaces/iinstallation';
import { from, Observable } from 'rxjs';
import { MatDialog } from '@angular/material';
import { IPlotResponse } from '../interfaces/iplot';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private API = environment.api_url;

  private handler: Handler;

  private hubConnection: HubConnection;
  private isSocketLive: boolean;

  onUnitsUpdate: EventEmitter<IUnit[]> = new EventEmitter<IUnit[]>();

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandler,
    private dialogRef: MatDialog
  ) {
    this.handler = this.errorHandler.createErrorHandler('DataService');
    this.createSocketConnection();
    this.registerOnServerEvents();
  }

  //#region getInstallations
  /**
   * Get all available installations from the backend
   * @returns Observable with the last_update time and Array of installations
   */
  getInstallations = () => {
    // Create a subinstance of the handler with the operation that failed and the return object
    const handler = this.handler('Get Installations', []);
    return this.http.get<IInstallation[]>(this.API + '/installations').pipe(
      tap(() => this.dialogRef.closeAll()),
      retryWhen(handler.retryStrategy),
      catchError(handler.handleError)
    );
  }
  //#endregion
  //#region getClients
  /**
   * Get all clients for a specific installation
   * @param id - the installation id
   * @returns Observable with the last_update time and Array of clients
   */
  getClients = (id: number) => {
    // Create a subinstance of the handler with the operation that failed and the return object
    const handler = this.handler('Get Clients', []);
    return this.http
      .get<IClient[]>(this.API + `/clients?installationId=${id}`)
      .pipe(
        tap(() => this.dialogRef.closeAll()),
        retryWhen(handler.retryStrategy),
        catchError(handler.handleError)
      );
  }
  //#endregion
  //#region Units
  /**
   * Get all units for a specific installation -> client
   * @param installation_id - the installation id
   * @param client_id - the client id
   * @returns Observable with an Array of all units
   */
  getUnits = (installation_id: number, client_id: number) => {
    // Create a subinstance of the handler with the operation that failed and the return object
    const handler = this.handler('Get Units', []);
    return this.http
      .get<IUnit[]>(
        this.API +
          `/units?installationId=${installation_id}&clientId=${client_id}`
      )
      .pipe(
        tap(() => this.dialogRef.closeAll()),
        retryWhen(handler.retryStrategy),
        catchError(handler.handleError)
      );
  }

  /**
   * Get performance plot for a specific unit
   * @param installationId - the installation id
   * @param unitId - the unit id
   * @param weeksAgo - number of weeks back for the requested data
   * @returns Observable with unit information, historical and predictions data
   */
  getUnitPlot = (
    installationId: number,
    unitId: number,
    weeksAgo: number = 2
  ): Observable<IPlotResponse> => {
    // Create a subinstance of the handler with the operation that failed and the return object
    const handler = this.handler('Get Unit Plot', {});
    return this.http
      .get<IPlotResponse>(
        this.API +
          `/unit/plot?installationId=${installationId}&unitId=${unitId}&weeksAgo=${weeksAgo}`
      )
      .pipe(
        tap(() => this.dialogRef.closeAll()),
        retryWhen(handler.retryStrategy),
        catchError(handler.handleError)
      );
  }
  /**
   * Trigger model retrain
   * @param installationId - the installation id
   * @param unitId - the unit id
   * @returns Observable with status 200 if the retrain is triggered
   */
  retrainUnit = (installationId: number, unitId: number): Observable<any> => {
    const handler = this.handler('Trigger model retrain', {});
    return this.http.get<any>(this.API + `/unit/train?installationId=${installationId}&unitId=${unitId}`).pipe(
      tap(() => this.dialogRef.closeAll()),
      retryWhen(handler.retryStrategy),
      catchError(handler.handleError)
    );
  }

  //#endregion
  //#region SocketConnection
  /**
   * Create the socket connection
   */
  private createSocketConnection = (): void => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.API + '/hub/units')
      .build();
  }

  /**
   * Listens for the 'UnitsUpdate' message
   */
  private registerOnServerEvents = (): void => {
    this.hubConnection.on('UnitsUpdate', (units: IUnit[]) =>
      this.onUnitsUpdate.next(units)
    );

    this.hubConnection.onclose(() => location.reload());
  }

  /**
   * Start the socket connection
   */
  startConnection = (): void => {
    if (this.isSocketLive) {
      return;
    }
    // Create a subinstance of the handler with the operation that failed
    const handler = this.handler('Start socket connection');

    from(this.hubConnection.start())
      .pipe(
        tap(() => this.dialogRef.closeAll()),
        retryWhen(handler.retryStrategy),
        catchError(handler.handleError)
      )
      .subscribe(r => {
        // If there is a response it's from the error handler
        if (!r) {
          this.isSocketLive = true;
          console.log('Socket connection started');
        }
      });
  }
  //#endregion
}
