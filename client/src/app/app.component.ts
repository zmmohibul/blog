import { Component, OnInit } from '@angular/core';
import { AuthService, User } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'client';

  constructor(private authService: AuthService) {}

  ngOnInit() {
    let user = localStorage.getItem('user');
    user = JSON.parse(user);
    if (user) {
      this.authService.currentUser$.next({ username: user["username"], createdAt: user["createdAt"], token: user["token"] });

      this.authService.currentUser$.subscribe((user: User) => {
        console.log(user);
        console.log(`Username: ${user.username}, Token: ${user.token}`);
      })
    }
  }
}
