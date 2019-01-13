import { Component, Input } from '@angular/core';
import { UnitStatus } from 'src/app/enums/unit-status.enum';

@Component({
  selector: 'app-performance-indicator',
  templateUrl: './performance-indicator.component.html',
  styleUrls: ['./performance-indicator.component.scss']
})
export class PerformanceIndicatorComponent {
  // Left side is the component property -> Right side is the imported Enum
  UnitStatus = UnitStatus;

  @Input('status')
  status: UnitStatus;

  constructor() {}
}
