import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import {
  LoginRequest,
  RegisterRequest,
  AuthResponse
} from '../../shared/models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7118/api/Auth';

  constructor(private http: HttpClient) {}

  // =========================
  // LOGIN
  // =========================
  login(request: LoginRequest): Observable<AuthResponse> {

    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/login`,
        request
      )
      .pipe(

        tap((response: AuthResponse) => {

          localStorage.setItem(
            'token',
            response.token
          );

          localStorage.setItem(
            'user',
            JSON.stringify(response)
          );

          console.log('Token saved successfully');

        })

      );
  }

  // =========================
  // REGISTER
  // =========================
  register(request: RegisterRequest): Observable<AuthResponse> {

    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/register`,
        request
      );

  }

  // =========================
  // LOGOUT
  // =========================
  logout(): void {

    localStorage.removeItem('token');
    localStorage.removeItem('user');

  }

  // =========================
  // CHECK LOGIN
  // =========================
  isLoggedIn(): boolean {

    return !!localStorage.getItem('token');

  }

  // =========================
  // GET CURRENT USER
  // =========================
  getUser(): AuthResponse | null {

    const user = localStorage.getItem('user');

    if (!user) {
      return null;
    }

    try {

      return JSON.parse(user) as AuthResponse;

    } catch {

      return null;

    }

  }

  // =========================
  // GET USER ROLE
  // =========================
  getRole(): string | null {

    const user = this.getUser();

    return user?.role ?? null;

  }

  // =========================
  // GET USER ID
  // =========================
  getUserId(): number | null {

    const user = this.getUser();

    return user?.userId ?? null;

  }

  // =========================
  // GET TOKEN
  // =========================
  getToken(): string | null {

    return localStorage.getItem('token');

  }

}