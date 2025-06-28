import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { UserService } from './core/services/user.service';
import { SnackbarService } from './core/services/snackbar.service';
import { HabitService } from './core/services/habit.service';
import { HabitCompletionService } from './core/services/habit.completion.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    UserService,
    SnackbarService,
    HabitService,
    HabitCompletionService
  ]
};
