import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Activity } from '../../../shared/models/activity.model';
import { ActivityService } from '../../../core/services/activity.service';

@Component({
  selector: 'app-activities',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './activities.html',
  styleUrl: './activities.scss'
})
export class Activities implements OnInit {

  itineraryDayId!: number;

  activities: Activity[] = [];

  loading = false;
  saving = false;

  editingId: number | null = null;

  formData = {
    name: '',
    description: '',
    startTime: '',
    endTime: ''
  };

  constructor(
    private route: ActivatedRoute,
    private activityService: ActivityService
  ) {}

  ngOnInit(): void {

    this.itineraryDayId = Number(
      this.route.snapshot.paramMap.get('itineraryDayId')
    );

    this.loadActivities();
  }

  loadActivities(): void {

    this.loading = true;

    this.activityService
      .getByItineraryDay(this.itineraryDayId)
      .subscribe({

        next: (data: Activity[]) => {

          this.activities = data;

          this.loading = false;

          console.log('Activities:', data);
        },

        error: (error: unknown) => {

          console.error(
            'Error loading activities:',
            error
          );

          this.loading = false;
        }
      });
  }

  saveActivity(): void {

    if (!this.formData.name.trim()) {
      return;
    }

    this.saving = true;

    const activityData = {

      itineraryDayId: this.itineraryDayId,

      name: this.formData.name.trim(),

      description: this.formData.description.trim(),

      startTime: this.formData.startTime,

      endTime: this.formData.endTime
    };

    if (this.editingId === null) {

      this.activityService
        .create(activityData)
        .subscribe({

          next: (data: Activity) => {

            console.log(
              'Activity created:',
              data
            );

            this.activities.push(data);

            this.resetForm();

            this.saving = false;
          },

          error: (error: unknown) => {

            console.error(
              'Error creating activity:',
              error
            );

            this.saving = false;
          }
        });

    } else {

      this.activityService
        .update(
          this.editingId,
          activityData
        )
        .subscribe({

          next: (data: Activity) => {

            console.log(
              'Activity updated:',
              data
            );

            const index =
              this.activities.findIndex(
                activity =>
                  activity.id === this.editingId
              );

            if (index !== -1) {
              this.activities[index] = data;
            }

            this.resetForm();

            this.saving = false;
          },

          error: (error: unknown) => {

            console.error(
              'Error updating activity:',
              error
            );

            this.saving = false;
          }
        });
    }
  }

  editActivity(activity: Activity): void {

    this.editingId = activity.id;

    this.formData = {

      name: activity.name,

      description: activity.description,

      startTime: this.formatTimeForInput(
        activity.startTime
      ),

      endTime: this.formatTimeForInput(
        activity.endTime
      )
    };

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  deleteActivity(activity: Activity): void {

    const confirmed = confirm(
      `Are you sure you want to delete "${activity.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.activityService
      .delete(activity.id)
      .subscribe({

        next: () => {

          console.log(
            'Activity deleted:',
            activity.id
          );

          this.activities =
            this.activities.filter(
              item =>
                item.id !== activity.id
            );
        },

        error: (error: unknown) => {

          console.error(
            'Error deleting activity:',
            error
          );
        }
      });
  }

  resetForm(): void {

    this.editingId = null;

    this.formData = {

      name: '',

      description: '',

      startTime: '',

      endTime: ''
    };
  }

  formatTimeForInput(time: string): string {

    if (!time) {
      return '';
    }

    return time.substring(0, 5);
  }
}