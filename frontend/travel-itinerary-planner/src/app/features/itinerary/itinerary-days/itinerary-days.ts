import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RouterLink } from '@angular/router';

import { ItineraryDay } from '../../../shared/models/itinerary-day.model';
import { ItineraryDayService } from '../../../core/services/itinerary-day.service';

@Component({
  selector: 'app-itinerary-days',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './itinerary-days.html',
  styleUrl: './itinerary-days.scss'
})
export class ItineraryDays implements OnInit {

  // All itinerary days belonging to this trip
  days: ItineraryDay[] = [];

  // Comes from /itinerary-days/:tripId
  tripId!: number;

  // Form object
  newDay: ItineraryDay = {
    id: 0,
    tripId: 0,
    dayNumber: 1,
    date: '',
    notes: ''
  };

  // null = adding
  // number = editing existing day
  editingId: number | null = null;

  loading = false;
  saving = false;

  constructor(
    private route: ActivatedRoute,
    private itineraryDayService: ItineraryDayService
  ) {}

  ngOnInit(): void {

    // Read tripId from URL
    this.tripId = Number(
      this.route.snapshot.paramMap.get('tripId')
    );

    if (!this.tripId) {
      console.error('Invalid trip ID');
      return;
    }

    // Set trip ID in form
    this.newDay.tripId = this.tripId;

    // Load days
    this.loadDays();
  }

  // ----------------------------------------------------
  // GET DAYS
  // ----------------------------------------------------

  loadDays(): void {

    this.loading = true;

    this.itineraryDayService
      .getByTrip(this.tripId)
      .subscribe({

        next: (data) => {

          this.days = data;

          // Sort by day number
          this.days.sort(
            (a, b) => a.dayNumber - b.dayNumber
          );

          this.loading = false;

          console.log('Itinerary Days:', this.days);
        },

        error: (error) => {

          this.loading = false;

          console.error(
            'Failed to load itinerary days:',
            error
          );
        }
      });
  }

  // ----------------------------------------------------
  // ADD / UPDATE
  // ----------------------------------------------------

  saveDay(): void {

    if (!this.newDay.date) {
      alert('Please select a date.');
      return;
    }

    if (!this.newDay.notes.trim()) {
      alert('Please enter notes for this day.');
      return;
    }

    this.saving = true;

    // UPDATE
    if (this.editingId !== null) {

      this.itineraryDayService
        .update(this.editingId, this.newDay)
        .subscribe({

          next: () => {

            console.log('Day updated successfully');

            this.saving = false;

            this.resetForm();

            this.loadDays();
          },

          error: (error) => {

            this.saving = false;

            console.error(
              'Update failed:',
              error
            );
          }
        });

      return;
    }

    // CREATE
    this.itineraryDayService
      .create(this.newDay)
      .subscribe({

        next: (createdDay) => {

          console.log(
            'Day created:',
            createdDay
          );

          this.saving = false;

          this.resetForm();

          this.loadDays();
        },

        error: (error) => {

          this.saving = false;

          console.error(
            'Create failed:',
            error
          );
        }
      });
  }

  // ----------------------------------------------------
  // EDIT
  // ----------------------------------------------------

  editDay(day: ItineraryDay): void {

    this.editingId = day.id;

    this.newDay = {
      id: day.id,
      tripId: day.tripId,
      dayNumber: day.dayNumber,

      // HTML date input needs yyyy-MM-dd
      date: this.formatDateForInput(day.date),

      notes: day.notes
    };

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // ----------------------------------------------------
  // DELETE
  // ----------------------------------------------------

  deleteDay(id: number): void {

    const confirmed = confirm(
      'Are you sure you want to delete this day?'
    );

    if (!confirmed) {
      return;
    }

    this.itineraryDayService
      .delete(id)
      .subscribe({

        next: () => {

          console.log('Day deleted');

          this.loadDays();
        },

        error: (error) => {

          console.error(
            'Delete failed:',
            error
          );
        }
      });
  }

  // ----------------------------------------------------
  // RESET FORM
  // ----------------------------------------------------

  resetForm(): void {

    this.editingId = null;

    this.newDay = {
      id: 0,

      // NEVER hardcode trip ID
      tripId: this.tripId,

      // Automatically calculate next day
      dayNumber: this.getNextDayNumber(),

      date: '',

      notes: ''
    };
  }

  // ----------------------------------------------------
  // NEXT DAY NUMBER
  // ----------------------------------------------------

  getNextDayNumber(): number {

    if (this.days.length === 0) {
      return 1;
    }

    const maxDayNumber = Math.max(
      ...this.days.map(day => day.dayNumber)
    );

    return maxDayNumber + 1;
  }

  // ----------------------------------------------------
  // DATE FORMAT
  // ----------------------------------------------------

  private formatDateForInput(date: string): string {

    if (!date) {
      return '';
    }

    return date.substring(0, 10);
  }
}