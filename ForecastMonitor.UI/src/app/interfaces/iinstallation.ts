import { UnitStatus } from '../enums/unit-status.enum';
import { IClient } from './iclient';
import { Observable } from 'rxjs';
import { IStatusReport } from './istatus-report';

export interface IInstallation {
  id: number;
  name: string;
  status?: UnitStatus;
  clients$?: Observable<IClient[]>;

  updateInstallationStatus?(report: IStatusReport): void;
}
