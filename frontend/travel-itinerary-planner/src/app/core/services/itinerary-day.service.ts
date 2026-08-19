import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ItineraryDay } from '../../shared/models/itinerary-day.model';

@Injectable({
  providedIn: 'root'
})
export class ItineraryDayService {

  private apiUrl = 'https://localhost:7118/api/ItineraryDay';

  constructor(private http: HttpClient) {}

  getByTrip(tripId: number): Observable<ItineraryDay[]> {
    return this.http.get<ItineraryDay[]>(
      `${this.apiUrl}/trip/${tripId}`
    );
  }

  getById(id: number): Observable<ItineraryDay> {
    return this.http.get<ItineraryDay>(
      `${this.apiUrl}/${id}`
    );
  }

  create(day: ItineraryDay): Observable<ItineraryDay> {
    return this.http.post<ItineraryDay>(
      this.apiUrl,
      day
    );
  }

  update(id: number, day: ItineraryDay): Observable<ItineraryDay> {
    return this.http.put<ItineraryDay>(
      `${this.apiUrl}/${id}`,
      day
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}