import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DestinationService } from '../../core/services/destination.service';
import { Destination } from '../../shared/models/destination.model';

@Component({
  selector: 'app-destinations',
  imports: [CommonModule, FormsModule],
  templateUrl: './destinations.html',
  styleUrl: './destinations.scss'
})
export class Destinations implements OnInit {

  destinations: Destination[] = [];

  newDestination: Destination = {
    id: 0,
    name: '',
    city: '',
    country: '',
    description: '',
    createdByUserId: 0

  };

  constructor(private destinationService: DestinationService) {}

  ngOnInit(): void {
    this.loadDestinations();
  }

  deleteDestination(id: number): void {
  this.destinationService.delete(id).subscribe({
    next: () => {
      this.destinations = this.destinations.filter(d => d.id !== id);
      console.log('Destination deleted');
    },
    error: (error: any) => {
      console.error('Delete failed:', error);
    }
  });
}

  loadDestinations(): void {
    this.destinationService.getAll().subscribe({
      next: (data: Destination[]) => {
        this.destinations = data;
      },
      error: (error: any) => {
        console.error('Failed to load destinations:', error);
      }
    });
  }

  createDestination(): void {

    this.destinationService.create(this.newDestination).subscribe({
      next: (createdDestination: Destination) => {

        console.log('Created:', createdDestination);

        this.destinations.push(createdDestination);

        this.newDestination = {
          id: 0,
          name: '',
          city: '',
          country: '',
          description: '',
          createdByUserId: 0
        };
      },

      error: (error: any) => {
        console.error('Failed to create destination:', error);
      }
    });
  }

  editDestination(destination: Destination): void {
  const updatedDestination: Destination = {
    ...destination,
    name: prompt('Enter destination name:', destination.name) || destination.name
  };

  this.destinationService.update(destination.id, updatedDestination).subscribe({
    next: (data: Destination) => {
      const index = this.destinations.findIndex(
        d => d.id === destination.id
      );

      if (index !== -1) {
        this.destinations[index] = data;
      }

      console.log('Destination updated:', data);
    },
    error: (error: any) => {
      console.error('Update failed:', error);
    }
  });
}
}