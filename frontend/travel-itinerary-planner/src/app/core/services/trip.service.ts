import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Trip } from '../../shared/models/trip-model';

@Injectable({
  providedIn: 'root'
})
export class TripService {

  private apiUrl = 'https://localhost:7118/api/Trip';

  constructor(
    private http: HttpClient
  ) {}

  getAll(): Observable<Trip[]> {

    return this.http.get<Trip[]>(
      this.apiUrl
    );
  }

  getById(id: number): Observable<Trip> {

    return this.http.get<Trip>(
      `${this.apiUrl}/${id}`
    );
  }

  create(trip: {
    title: string;
    startDate: string;
    endDate: string;
  }): Observable<Trip> {

    return this.http.post<Trip>(
      this.apiUrl,
      trip
    );
  }

  update(
    id: number,
    trip: {
      title: string;
      startDate: string;
      endDate: string;
    }
  ): Observable<Trip> {

    return this.http.put<Trip>(
      `${this.apiUrl}/${id}`,
      trip
    );
  }

  delete(id: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}