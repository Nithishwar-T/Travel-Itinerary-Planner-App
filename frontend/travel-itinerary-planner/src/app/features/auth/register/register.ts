import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth';
import { RegisterRequest } from '../../../shared/models/auth.model';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {

  registerData: RegisterRequest = {
    fullName: '',
    email: '',
    password: ''
  };

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.registerData.fullName ||
      !this.registerData.email ||
      !this.registerData.password
    ) {
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    if (this.registerData.fullName.length > 100) {
      this.errorMessage = 'Full name cannot exceed 100 characters.';
      return;
    }

    if (this.registerData.password.length < 6) {
      this.errorMessage = 'Password must contain at least 6 characters.';
      return;
    }

    this.isLoading = true;

    this.authService.register(this.registerData).subscribe({

      next: (response) => {

        console.log('Registration successful:', response);

        this.isLoading = false;
        this.successMessage = 'Registration successful! Redirecting to login...';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1000);
      },

      error: (error) => {

        console.error('Registration failed:', error);

        this.isLoading = false;

        if (error.status === 409) {
          this.errorMessage =
            error.error?.message || 'Email is already registered.';
        }
        else if (error.status === 400) {
          this.errorMessage =
            'Please check your registration details.';
        }
        else {
          this.errorMessage =
            'Registration failed. Please try again.';
        }
      }

    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}