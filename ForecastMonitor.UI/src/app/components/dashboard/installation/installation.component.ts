import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable, Subject, concat } from 'rxjs';
import { shareReplay, map } from 'rxjs/operators';

// Import Services
import { DataService } from 'src/app/services/data.service';

// Import Interfaces
import { IClient } from 'src/app/interfaces/iclient';
import { IInstallation } from 'src/app/interfaces/iinstallation';

// Import Utilities
import { UnitStatus } from 'src/app/enums/unit-status.enum';
import * as _ from 'lodash';
import { IStatusReport } from 'src/app/interfaces/istatus-report';

@Component({
  selector: 'app-installation',
  templateUrl: './installation.component.html',
  styleUrls: ['./installation.component.scss']
})
export class InstallationComponent implements OnInit, IInstallation {
  id: number;
  name: string;
  status: UnitStatus;
  clients$: Observable<IClient[]>;

  @Input()
  private model: IInstallation;
  @Input()
  unitLabel: string;

  @Output()
  reportStatus = new EventEmitter<IStatusReport>();

  private clients: IClient[] = [];
  private updater: Subject<IClient[]> = new Subject<IClient[]>();

  constructor(private dataService: DataService) {}

  ngOnInit() {
    if (this.model) {
      this.id = this.model.id;
      this.name = this.model.name;
    }

    const initialSource = this.dataService.getClients(this.id);
    const updateSource = this.updater.asObservable();

    this.clients$ = concat(initialSource, updateSource).pipe(
      map(clients => (this.clients = clients)),
      shareReplay(1)
    );
  }

  updateInstallationStatus = (report: IStatusReport): void => {
    // Reduce function for getting the worst performing unit
    const getWorstPerformance = (
      _worst: UnitStatus,
      current: UnitStatus
    ): UnitStatus => (current < _worst ? current : _worst);

    // Find and update the local client
    const target = this.clients.find(c => c.id === report.id);
    target.status = report.status;

    const isLast = this.clients.indexOf(target) === this.clients.length - 1;

    if (isLast) {
      // Update the installation status
      this.status = this.clients.map(c => c.status).reduce(getWorstPerformance);
      // Order the clients by status
      const ordered = _.orderBy(this.clients, 'status', 'asc');
      this.updater.next(ordered);

      // Report the installation status
      this.reportStatus.next({
        id: this.id,
        status: this.status
      });
    }
  }
}
