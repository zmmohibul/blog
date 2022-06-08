import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, User } from './auth/auth.service';
import { PresenceService } from './auth/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'client';

  constructor(private authService: AuthService, private router: Router, private presenceService: PresenceService) {}

  ngOnInit() {
    let user: User = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.authService.setCurrentUser(user);
      this.presenceService.createHubConnection(user);

      this.router.navigateByUrl('/posts');
    }
  }
}
