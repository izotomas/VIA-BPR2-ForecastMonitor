import { Component, OnInit, OnDestroy } from '@angular/core';

// Import Services
import { DataService } from '../../services/data.service';
import { TranslationService } from '../../services/translation.service';

// Import Interfaces
import { IInstallation } from '../../interfaces/iinstallation';

// Import Utils
import * as _ from 'lodash';
import { Subscription, Observable, Subject, concat } from 'rxjs';
import { shareReplay, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
  installations$: Observable<IInstallation[]>;
  pageHeader: string;
  unitLabel: string;

  subscriptions: Subscription[] = [];

  private updater: Subject<IInstallation[]> = new Subject<IInstallation[]>();
  private installations: IInstallation[] = [];

  constructor(
    private dataService: DataService,
    private translationService: TranslationService
  ) {
    const translationSub = this.translationService
      .fetchTranslations()
      .subscribe(translation => {
        this.pageHeader = translation.dashboard.pageHeader;
        this.unitLabel = translation.model.label;
      });
    this.subscriptions.push(translationSub);
  }

  ngOnInit() {
    console.log('--- Dashboard Initialised ---');

    // Get the available installations
    const initialSource = this.dataService
      .getInstallations()
      .pipe(tap(() => this.dataService.startConnection()));
    const updateSource = this.updater.asObservable();

    this.installations$ = concat(initialSource, updateSource).pipe(
      map(installations => (this.installations = installations)),
      shareReplay(1)
    );
  }

  ngOnDestroy() {
    // Unsubscribe from all observables to prevent memory leaks
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  orderInstallations(installation: IInstallation): void {
    const target = this.installations.find(i => i.id === installation.id);
    target.status = installation.status;

    const isLast =
      this.installations.indexOf(target) === this.installations.length - 1;

    if (isLast) {
      const ordered = _.orderBy(this.installations, 'status', 'asc');
      this.updater.next(ordered);
    }
  }
}
