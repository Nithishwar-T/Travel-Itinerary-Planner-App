import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TripService } from '../../core/services/trip.service';
import { Trip } from '../../shared/models/trip-model';

@Component({
  selector: 'app-trips',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trips.html',
  styleUrl: './trips.scss'
})
export class Trips implements OnInit {

  trips: Trip[] = [];

  newTrip = {
    title: '',
    startDate: '',
    endDate: ''
  };

  constructor(private tripService: TripService) {}

  ngOnInit(): void {
    this.loadTrips();
  }

  loadTrips(): void {
    this.tripService.getAll().subscribe({
      next: (data: Trip[]) => {
        this.trips = data;
        console.log('Trips:', data);
      },
      error: (error: any) => {
        console.error('Failed to load trips:', error);
      }
    });
  }

  addTrip(): void {
    this.tripService.create(this.newTrip).subscribe({
      next: (data: Trip) => {
        console.log('Trip created:', data);

        this.trips.push(data);

        this.newTrip = {
          title: '',
          startDate: '',
          endDate: ''
        };
      },
      error: (error: any) => {
        console.error('Create trip failed:', error);
      }
    });
  }

  editTrip(trip: Trip): void {

    const title = prompt(
      'Enter trip title:',
      trip.title
    );

    if (!title) {
      return;
    }

    const updatedTrip = {
      title: title,
      startDate: trip.startDate,
      endDate: trip.endDate
    };

    this.tripService.update(trip.id, updatedTrip).subscribe({
      next: (data: Trip) => {

        const index = this.trips.findIndex(
          t => t.id === trip.id
        );

        if (index !== -1) {
          this.trips[index] = data;
        }

        console.log('Trip updated:', data);
      },
      error: (error: any) => {
        console.error('Update trip failed:', error);
      }
    });
  }

  deleteTrip(id: number): void {

    this.tripService.delete(id).subscribe({
      next: () => {
        this.trips = this.trips.filter(
          trip => trip.id !== id
        );

        console.log('Trip deleted');
      },
      error: (error: any) => {
        console.error('Delete trip failed:', error);
      }
    });
  }
}