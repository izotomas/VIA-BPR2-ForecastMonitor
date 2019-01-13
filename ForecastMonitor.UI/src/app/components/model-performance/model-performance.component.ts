import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, concat, Subscription } from 'rxjs';
import { map, shareReplay, switchMap, tap } from 'rxjs/operators';

import { environment } from '../../../environments/environment';

// Import Utilities
import * as moment from 'moment';

// Import Services
import { DataService } from 'src/app/services/data.service';

// Import Interfaces
import { IPlot, IPlotResponse } from 'src/app/interfaces/iplot';
import { ToasterService } from 'angular2-toaster';

@Component({
  selector: 'app-model-performance',
  templateUrl: './model-performance.component.html',
  styleUrls: ['./model-performance.component.scss']
})
export class ModelPerformanceComponent implements OnInit, OnDestroy {
  // Line Graph Options
  autoScale = true;

  showXAxis = true;
  showXAxisLabel = true;
  xAxisLabel = 'Day';
  xAxisTicks: Date[];

  showYAxis = true;
  showYAxisLabel = true;
  yAxisLabel = 'Occupancy';

  colorScheme = {
    domain: environment.modelPerformanceColorScheme
  };

  plot$: Observable<IPlot>;

  // Time interval
  interval: number;
  minInterval: number;
  maxInterval: number;

  fetchingData: boolean;
  retrainRequest: boolean;

  private installation_id: number;
  private unit_id: number;

  private updater: Subject<number> = new Subject<number>();
  private subscriptions: Subscription[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private dataService: DataService,
    private toastr: ToasterService
  ) {}

  ngOnInit() {
    // Fetch interval data from the environmonet
    this.interval = environment.defaultInterval;
    this.minInterval = environment.minInterval;
    this.maxInterval = environment.maxInterval;

    // Generate the xAxisTicks based on the configured interval
    // this.xAxisTicks = this.generateXAxisTicks(this.interval);

    this.activatedRoute.params.subscribe(params => {
      if (params) {
        this.installation_id = params.installation_id;
        this.unit_id = params.unit_id;

        // Setup the initial data source
        const initialSource = this.dataService.getUnitPlot(
          this.installation_id,
          this.unit_id,
          this.interval
        );
        // Setup the update data source
        const updateSource = this.updater.asObservable().pipe(
          switchMap(weeks => {
            this.fetchingData = true; // Hide the graph and show loading animation
            this.interval = weeks;
            return this.dataService.getUnitPlot(
              this.installation_id,
              this.unit_id,
              weeks
            );
          }),
          tap(() => (this.fetchingData = false))
        );

        // Combine the two data sources
        this.plot$ = concat(initialSource, updateSource).pipe(
          map(plot => {
            this.generateXAxisTicks(this.interval, this.findLastDate(plot));
            return plot;
          }),
          map(this.mapPlot),
          shareReplay(1)
        );
      }
    });
  }

  ngOnDestroy() {
    // Unsubscribe from all observables to preven memory leaks
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  findLastDate = (plot: IPlotResponse): Date => {
    const historical = plot.historical;
    const predictions = plot.predictions;

    if (
      (!historical || !historical.length) &&
      (!predictions || !predictions.length)
    ) {
      return new Date();
    }

    let lastHistorical, lastPrediction;

    if (historical && historical.length) {
      lastHistorical = historical.reduce(
        (lastDate, item) =>
          moment(lastDate).isBefore(moment(item.x)) ? moment(item.x) : lastDate,
        moment(historical[0].x)
      );
    }

    if (predictions && predictions.length) {
      lastPrediction = predictions.reduce(
        (lastDate, item) =>
          moment(lastDate).isBefore(moment(item.x)) ? moment(item.x) : lastDate,
        moment(predictions[0].x)
      );
    }
    if (!lastHistorical) {
      return lastPrediction.toDate();
    }
    if (!lastPrediction) {
      return lastHistorical.toDate();
    }

    return (lastHistorical.isBefore(lastPrediction)
      ? lastPrediction
      : lastHistorical
    ).toDate();
  }

  mapPlot = (plot: IPlotResponse): IPlot => {
    const mapSeries = el => ({
      name: new Date(el.x),
      value: Number(el.y.toFixed(3))
    });

    return {
      unit_key: plot.unit_key,
      unit_name: plot.name,
      data: [
        {
          name: 'Historical',
          series: plot.historical ? plot.historical.map(mapSeries) : []
        },
        {
          name: 'Predictions',
          series: plot.predictions ? plot.predictions.map(mapSeries) : []
        }
      ],
      hasData: !!(
        (plot.historical && plot.historical.length) ||
        (plot.predictions && plot.predictions.length)
      )
    };
  }

  generateXAxisTicks = (weeksAgo: number = 2, lastDate: Date): Date[] => {
    const dates = [];

    const currDate = moment()
      .subtract(weeksAgo, 'weeks')
      .startOf('day');

    while (currDate.add(1, 'days').diff(lastDate) < 0) {
      dates.push(currDate.clone().toDate());
    }
    return dates;
  }

  xAxisFormat = (event: Date): string => moment(event).format('ddd DD-MM');

  onIntervalChange = (weeksAgo: number): void => this.updater.next(weeksAgo);

  retrainModel = () => {
    this.retrainRequest = true;
    const sub = this.dataService
      .retrainUnit(this.installation_id, this.unit_id)
      .subscribe({
        next: () => this.toastr.pop('success', 'Retrain request was send'),
        complete: () => (this.retrainRequest = false)
      });
    this.subscriptions.push(sub);
  }
}
