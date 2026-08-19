import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {

  email = '';

  password = '';

  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.errorMessage = '';

    this.authService
      .login({
        email: this.email,
        password: this.password
      })
      .subscribe({

        next: (response) => {

          console.log('Login successful:', response);

          this.router.navigate(['/dashboard']);

        },

        error: (error) => {

          console.error('Login failed:', error);

          this.errorMessage =
            error.error?.message ||
            'Login failed';

        }

      });

  }

}