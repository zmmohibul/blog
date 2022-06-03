import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BehaviorSubject } from 'rxjs';


export interface User {
  userName: string;
  createdAt: Date;
  token: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  rootUrl = 'https://localhost:5000/api';
  currentUser$ = new BehaviorSubject<User>(null);

  constructor(private http: HttpClient) { }
}
