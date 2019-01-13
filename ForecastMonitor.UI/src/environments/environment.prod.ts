export const environment = {
  production: true,
  api_url: 'http://dabai-test1.systematicgroup.local:56166',
  retryAttempts: 3,
  retryDelay: 1000,
  defaultInterval: 2, // Default 2 weeks
  minInterval: 1, // Minimum weeks
  maxInterval: 10, // Maximum weeks
  modelPerformanceColorScheme: ['gray', '#4c84b1'] // [predictions, historical]
};
