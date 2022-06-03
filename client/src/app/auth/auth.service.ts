import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';


export interface User {
  userName: string;
  createdAt: Date;
  token: string;
}

interface UsernameExistsResponse {
  available: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  rootUrl = 'https://localhost:5001/api/auth';
  currentUser$ = new BehaviorSubject<User>(null);

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
}
