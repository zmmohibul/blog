import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';


export interface User {
  username: string;
  createdAt: Date;
  token: string;
}

interface UsernameExistsResponse {
  available: boolean;
}

export interface RegisterCredentials {
  username: string;
  password: string;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  rootUrl = 'https://localhost:5001/api/auth';
  currentUser$ = new BehaviorSubject<User>(null);
  signedIn$ = new BehaviorSubject(false);

  constructor(private http: HttpClient) { }

  usernameExists(username: string) {
    const options = {
      params: new HttpParams().set('username', username)
    }
    return this.http.get<UsernameExistsResponse>(
      `${this.rootUrl}/userexist`,
      options
    );
  }

  register(registerCredentials: RegisterCredentials) {
    return this.http
      .post<User>(`${this.rootUrl}/register`, registerCredentials)
      .pipe(tap((user: User) => {
        localStorage.setItem('user', JSON.stringify(user));
        this.currentUser$.next(user);
        this.signedIn$.next(true);
      }));
  }

  login(loginCredentials: LoginCredentials) {
    return this.http.post<User>(`${this.rootUrl}/login`, loginCredentials)
      .pipe(tap((user: User) => {
        localStorage.setItem('user', JSON.stringify(user));
        this.currentUser$.next(user);
        this.signedIn$.next(true);
      }));
  }

  getAllUsers() {
    return this.http.get<User[]>(`${this.rootUrl}/allusers`);
  }
}
