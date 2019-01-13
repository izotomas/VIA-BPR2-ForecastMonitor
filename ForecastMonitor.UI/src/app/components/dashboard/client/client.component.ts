import {
  Component,
  OnInit,
  Input,
  ChangeDetectorRef,
  EventEmitter,
  Output
} from '@angular/core';
import { Observable, concat } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { DataService } from 'src/app/services/data.service';

// Import Interfaces
import { IClient } from 'src/app/interfaces/iclient';
import { IUnit } from 'src/app/interfaces/iunit';

// Import Utilities
import { UnitStatus } from 'src/app/enums/unit-status.enum';
import * as _ from 'lodash';
import { IStatusReport } from 'src/app/interfaces/istatus-report';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit, IClient {
  id: number;
  name: string;
  installation_id?: number;
  status?: UnitStatus;
  activeModels$?: Observable<IUnit[]>;

  UnitStatus = UnitStatus;

  @Input()
  unitLabel: string;

  @Input()
  private model: IClient;

  @Output()
  reportStatus = new EventEmitter<IStatusReport>();

  constructor(
    private cd: ChangeDetectorRef,
    private dataService: DataService
  ) {}

  ngOnInit() {
    if (this.model) {
      this.id = this.model.id;
      this.name = this.model.name;
      this.installation_id = this.model.installation_id;
      this.status = this.model.status;
    }
    const initialSource = this.dataService.getUnits(
      this.installation_id,
      this.id
    );

    const updateSource = this.dataService.onUnitsUpdate.pipe(
      map(units => units.filter(u => u.client_id === this.id)),
      tap(units =>
        console.log('Units Update: ' + units.length + ' units received')
      )
    );

    this.activeModels$ = concat(initialSource, updateSource).pipe(
      map(this.orderUnits),
      tap(this.updateClientStatus)
    );
  }

  private orderUnits = (data: IUnit[]): IUnit[] => {
    data = data.map(u => {
      u.performance = UnitStatus[u.performance];
      return u;
    });

    // Order the units
    const order = _.orderBy(
      data,
      ['performance', ({ mae }) => (isNaN(mae) ? '' : mae)],
      ['asc', 'desc']
    );

    return order.map(u => {
      u.performance = UnitStatus[u.performance];
      u.installation_id = this.installation_id;
      return u;
    });
  }

  /**
   * Update the client overall status if any of the units starts failing
   * @param units - all the units of the client
   */
  updateClientStatus = (units: IUnit[]): void => {
    // If the client found have no status set yet consider it as Not enough data
    if (!this.status) {
      this.status = UnitStatus.Grey;
    }
    // Reduce function for getting the worst performing unit
    const getWorstPerformance = (
      _worst: UnitStatus,
      current: UnitStatus
    ): UnitStatus => (current < _worst ? current : _worst);

    // Update the client status
    this.status = units
      .map(u => UnitStatus[u.performance])
      .reduce(getWorstPerformance, UnitStatus.Grey);

    // Report the status to the installation
    this.reportStatus.next({
      id: this.id,
      status: this.status
    });

    // Force change detection to update the view
    this.cd.detectChanges();
  }
}
