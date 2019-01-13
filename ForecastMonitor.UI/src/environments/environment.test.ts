export const environment = {
  production: false,
  api_url: 'http://localhost:50000',
  retryAttempts: -1,
  retryDelay: 1000,
  defaultInterval: 2, // Default 2 weeks
  minInterval: 1, // Minimum weeks
  maxInterval: 10, // Maximum weeks
  modelPerformanceColorScheme: ['gray', '#4c84b1'] // [predictions, historical]
};
