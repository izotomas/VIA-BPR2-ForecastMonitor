import { IUnit } from './iunit';
import { UnitStatus } from '../enums/unit-status.enum';
import { Observable } from 'rxjs';

export interface IClient {
  id: number;
  name: string;
  installation_id?: number;
  status?: UnitStatus;
  activeModels$?: Observable<IUnit[]>;

  updateClientStatus?(units: IUnit[]): void;
}
