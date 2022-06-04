import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, User } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'client';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    let user = localStorage.getItem('user');
    user = JSON.parse(user);
    if (user) {
      this.authService.currentUser$.next({ username: user["username"], createdAt: user["createdAt"], token: user["token"] });
      this.authService.signedIn$.next(true);

      this.router.navigateByUrl('/posts');
    }
  }
}
