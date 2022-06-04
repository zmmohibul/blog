import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService, User } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  currentUser$: BehaviorSubject<User> = new BehaviorSubject(null);

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User) => {
      this.currentUser$.next(user);
    });
  }

  onLogout() {
    this.currentUser$.next(null);
    this.authService.currentUser$.next(null);
    this.authService.signedIn$.next(false);
    localStorage.clear();
    this.router.navigateByUrl("/");
  }

}