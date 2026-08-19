import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Destination } from '../../shared/models/destination.model';

@Injectable({
  providedIn: 'root'
})
export class DestinationService {

  private apiUrl = 'https://localhost:7118/api/Destination';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Destination[]> {
    return this.http.get<Destination[]>(this.apiUrl);
  }

  delete(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/${id}`);
}
  create(destination: Destination): Observable<Destination> {
  return this.http.post<Destination>(this.apiUrl, destination); 
}
update(id: number, destination: Destination): Observable<Destination> {
  return this.http.put<Destination>(
    `${this.apiUrl}/${id}`,
    destination
  );
}
}