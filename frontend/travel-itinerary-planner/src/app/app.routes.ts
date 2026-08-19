import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login')
        .then(m => m.Login)
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register')
        .then(m => m.Register)
  },

  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/dashboard/dashboard')
        .then(m => m.Dashboard)
  },

  {
    path: 'trips',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/trips/trips')
        .then(m => m.Trips)
  },

  {
    path: 'itinerary-days/:tripId',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/itinerary/itinerary-days/itinerary-days')
        .then(m => m.ItineraryDays)
  },

  {
    path: 'activities/:itineraryDayId',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/itinerary/activities/activities')
        .then(m => m.Activities)
  },

  {
    path: 'trips/:tripId/collaborators',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/collaborators/collaborators')
        .then(m => m.Collaborators)
  },

  {
    path: 'trips/:tripId/budget',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/budget/budget')
        .then(m => m.Budget)
  },

  {
    path: 'destinations',
    canActivate: [
      authGuard,
      roleGuard(['Admin', 'ContentCurator'])
    ],
    loadComponent: () =>
      import('./features/destinations/destinations')
        .then(m => m.Destinations)
  },

  {
    path: 'access-denied',
    loadComponent: () =>
      import('./features/access-denied/access-denied')
        .then(m => m.AccessDenied)
  },

  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];