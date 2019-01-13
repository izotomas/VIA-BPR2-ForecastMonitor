// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  api_url: 'http://dabai-test1.systematicgroup.local:56166',
  retryAttempts: -1, // -1 -> indefinately
  retryDelay: 5000, // Time in ms
  defaultInterval: 2, // Default 2 weeks
  minInterval: 1, // Minimum weeks
  maxInterval: 10, // Maximum weeks
  modelPerformanceColorScheme: ['gray', '#4c84b1'] // [predictions, historical]
};

/*
 * In development mode, for easier debugging, you can ignore zone related error
 * stack frames such as `zone.run`/`zoneDelegate.invokeTask` by importing the
 * below file. Don't forget to comment it out in production mode
 * because it will have a performance impact when errors are thrown
 */
import 'zone.js/dist/zone-error'; // Included with Angular CLI.
