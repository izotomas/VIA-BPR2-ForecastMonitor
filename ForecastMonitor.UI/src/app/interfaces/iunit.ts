import { UnitStatus } from '../enums/unit-status.enum';

export interface IUnit {
  id: number;
  name: string;
  client_id?: number;
  installation_id?: number;
  mae?: number;
  performance?: UnitStatus | string;

  last_update?: Date;
}
