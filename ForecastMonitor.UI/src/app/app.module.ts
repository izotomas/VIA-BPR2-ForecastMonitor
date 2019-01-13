import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { NgModule } from '@angular/core';
import { MaterialModule } from './material/material.module';
import { FlexLayoutModule } from '@angular/flex-layout';

import { NgxChartsModule } from '@swimlane/ngx-charts';

import { ToasterModule, ToasterService } from 'angular2-toaster';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

// Import Reusable Components
import { InstallationComponent } from './components/dashboard/installation/installation.component';
import { ClientComponent } from './components/dashboard/client/client.component';
import { ModelComponent } from './components/dashboard/model/model.component';
import { PerformanceIndicatorComponent } from './components/performance-indicator/performance-indicator.component';
import { ErrorComponent } from './modals/error/error.component';

// Import Services
import { DataService } from './services/data.service';
import { ErrorHandler } from './services/error-handler.service';
import { TranslationService } from './services/translation.service';

// Import Pipes
import { TruncatePipe } from './pipes/truncate.pipe';
import { ModelPerformanceComponent } from './components/model-performance/model-performance.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    ModelComponent,
    TruncatePipe,
    ErrorComponent,
    PerformanceIndicatorComponent,
    ClientComponent,
    InstallationComponent,
    ModelPerformanceComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MaterialModule,
    FlexLayoutModule,
    HttpClientModule,
    ToasterModule.forRoot(),
    NgxChartsModule
  ],
  entryComponents: [ErrorComponent],
  providers: [DataService, ErrorHandler, ToasterService, TranslationService],
  bootstrap: [AppComponent]
})
export class AppModule {}
