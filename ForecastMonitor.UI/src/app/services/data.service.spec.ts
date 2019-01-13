import { TestBed } from '@angular/core/testing';

import { DataService } from './data.service';

import { environment } from '../../environments/environment';

import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { ErrorHandler } from './error-handler.service';
import { IPlotResponse } from '../interfaces/iplot';
import { MaterialModule } from '../material/material.module';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { ErrorComponent } from '../modals/error/error.component';

describe('DataService', () => {
  let service: DataService;
  let backend: HttpTestingController;

  const API = environment.api_url;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MaterialModule, NoopAnimationsModule],
      declarations: [ErrorComponent],
      providers: [DataService, ErrorHandler]
    });

    TestBed.overrideModule(BrowserDynamicTestingModule, {
      set: {
        entryComponents: [ErrorComponent]
      }
    });

    service = TestBed.get(DataService);
    backend = TestBed.get(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have getInstallations method', () => {
    expect(service.getInstallations).toBeDefined();
  });
  it('should have getClients method', () => {
    expect(service.getClients).toBeDefined();
  });
  it('should have getUnits method', () => {
    expect(service.getUnits).toBeDefined();
  });
  it('should prepare the socket for connection', () => {
    expect(service['isSocketLive']).toBeUndefined();
    expect(service['hubConnection']).toBeDefined();
  });
  //#region Describe block getInstallations
  describe('getInstallations', () => {
    afterEach(() => {
      backend.verify();
    });

    it('should return correct object', () => {
      const fakeResponse = [];

      service.getInstallations().subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });
      // Expect correct endpoint to be hit
      const req = backend.expectOne({
        url: API + '/installations',
        method: 'GET'
      });
      // Send response
      req.flush(fakeResponse);
    });

    it('should return empty object on server error', () => {
      service.getInstallations().subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/installations',
        method: 'GET'
      });
      req.flush(
        { message: 'Server is down' },
        { status: 404, statusText: 'Bad request' }
      );
    });

    it('should return empty object on client side or network error', () => {
      service.getInstallations().subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/installations',
        method: 'GET'
      });
      req.error(
        new ErrorEvent('Mocked error', {
          message: 'Client side / Network error'
        })
      );
    });
  });
  //#endregion
  //#region Describe block getClients
  describe('getClients', () => {
    afterEach(() => {
      backend.verify();
    });

    it('should return correct object', () => {
      const fakeResponse = [];

      service.getClients(1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });
      // Expect correct endpoint to be hit
      const req = backend.expectOne({
        url: API + '/clients?installationId=1',
        method: 'GET'
      });
      // Send response
      req.flush(fakeResponse);
    });

    it('should return empty object on server error', () => {
      service.getClients(1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/clients?installationId=1',
        method: 'GET'
      });
      req.flush(
        { message: 'Server is down' },
        { status: 404, statusText: 'Bad request' }
      );
    });

    it('should return empty object on client side or network error', () => {
      service.getClients(1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/clients?installationId=1',
        method: 'GET'
      });
      req.error(
        new ErrorEvent('Mocked error', {
          message: 'Client side / Network error'
        })
      );
    });
  });
  //#endregion
  //#region Describe block getUnits
  describe('getUnits', () => {
    afterEach(() => {
      backend.verify();
    });

    it('should return correct object', () => {
      const fakeResponse = [];

      service.getUnits(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });
      // Expect correct endpoint to be hit
      const req = backend.expectOne({
        url: API + '/units?installationId=1&clientId=1',
        method: 'GET'
      });
      // Send response
      req.flush(fakeResponse);
    });

    it('should return empty object on server error', () => {
      service.getUnits(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/units?installationId=1&clientId=1',
        method: 'GET'
      });
      req.flush(
        { message: 'Server is down' },
        { status: 404, statusText: 'Bad request' }
      );
    });

    it('should return empty object on client side or network error', () => {
      service.getUnits(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Array));
      });

      const req = backend.expectOne({
        url: API + '/units?installationId=1&clientId=1',
        method: 'GET'
      });
      req.error(
        new ErrorEvent('Mocked error', {
          message: 'Client side / Network error'
        })
      );
    });
  });
  //#endregion
  //#region Describe block getUnitPlot - Test PP2-1
  describe('getUnitPlot', () => {
    afterEach(() => {
      backend.verify();
    });

    it('should return correct object', () => {
      const fakeResponse: IPlotResponse = {
        name: 'test',
        unit_key: 'test-key',
        historical: [],
        predictions: []
      };

      service.getUnitPlot(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response.unit_key).toBeDefined();
        expect(response.historical).toBeDefined();
        expect(response.historical).toEqual(jasmine.any(Array));
        expect(response.predictions).toBeDefined();
        expect(response.predictions).toEqual(jasmine.any(Array));
      });
      // Expect correct endpoint to be hit
      const req = backend.expectOne({
        url: API + '/unit/plot?installationId=1&unitId=1&weeksAgo=2',
        method: 'GET'
      });
      // Send response
      req.flush(fakeResponse);
    });

    it('should return empty object on server error', () => {
      service.getUnitPlot(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Object));
      });

      const req = backend.expectOne({
        url: API + '/unit/plot?installationId=1&unitId=1&weeksAgo=2',
        method: 'GET'
      });
      req.flush(
        { message: 'Server is down' },
        { status: 404, statusText: 'Bad request' }
      );
    });

    it('should return empty object on client side or network error', () => {
      service.getUnitPlot(1, 1).subscribe(response => {
        expect(response).toBeDefined();
        expect(response).toEqual(jasmine.any(Object));
      });

      const req = backend.expectOne({
        url: API + '/unit/plot?installationId=1&unitId=1&weeksAgo=2',
        method: 'GET'
      });
      req.error(
        new ErrorEvent('Mocked error', {
          message: 'Client side / Network error'
        })
      );
    });
  });
  //#endregion
  //#region Describe block retrainUnit - Test OS7
  describe('retrainUnit', () => {
    afterEach(() => {
      backend.verify();
    });

    it('should reach the correct endpoint', () => {
      service.retrainUnit(1, 1).subscribe(response => {
        expect(response).toBeDefined();
      });
      // Expect correct endpoint to be hit
      const req = backend.expectOne({
        url: API + '/unit/train?installationId=1&unitId=1',
        method: 'GET'
      });
      // Send response
      req.flush({});
    });
  });
  //#endregion
});
