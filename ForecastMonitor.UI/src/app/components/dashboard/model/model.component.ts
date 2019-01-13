import { Component, OnInit, Input } from '@angular/core';

// Import Intefaces
import { IUnit } from '../../../interfaces/iunit';

// Import Utilities
import { UnitStatus } from '../../../enums/unit-status.enum';

@Component({
  selector: 'app-model',
  templateUrl: './model.component.html',
  styleUrls: ['./model.component.scss']
})
export class ModelComponent implements OnInit, IUnit {
  @Input()
  private model: IUnit;

  @Input()
  label: string;

  id: number;
  client_id: number;
  installation_id: number;
  name: string;
  mae: number;
  performance: UnitStatus;

  loading = true;

  constructor() {}

  ngOnInit() {
    // Once we receive the model from the parrent populate the instance variables
    if (this.model) {
      this.id = this.model.id;
      this.client_id = this.model.client_id;
      this.installation_id = this.model.installation_id;
      this.name = this.model.name;

      if (typeof this.model.performance === 'string') {
        this.performance = UnitStatus[this.model.performance];
      }
      this.mae =
        this.model.mae && !isNaN(this.model.mae)
          ? Number(this.model.mae.toFixed(2))
          : this.model.mae;
      // End the loading screen when the data is retreived
      this.loading = false;
    }
  }
}
