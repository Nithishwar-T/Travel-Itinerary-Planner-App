import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Activity } from '../../shared/models/activity.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  private apiUrl = 'https://localhost:7118/api/Activity';

  constructor(private http: HttpClient) {}

  getByItineraryDay(itineraryDayId: number): Observable<Activity[]> {
    return this.http.get<Activity[]>(
      `${this.apiUrl}/itinerary-day/${itineraryDayId}`
    );
  }

  getById(id: number): Observable<Activity> {
    return this.http.get<Activity>(
      `${this.apiUrl}/${id}`
    );
  }

  create(activity: {
    itineraryDayId: number;
    name: string;
    description: string;
    startTime: string;
    endTime: string;
  }): Observable<Activity> {

    return this.http.post<Activity>(
      this.apiUrl,
      activity
    );
  }

  update(
    id: number,
    activity: {
      itineraryDayId: number;
      name: string;
      description: string;
      startTime: string;
      endTime: string;
    }
  ): Observable<Activity> {

    return this.http.put<Activity>(
      `${this.apiUrl}/${id}`,
      activity
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}