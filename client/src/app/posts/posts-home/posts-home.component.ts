import { Component, OnInit } from '@angular/core';
import { AuthService, User } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss']
})
export class PostsHomeComponent implements OnInit {
  users: User[] = [];

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.getAllUsers().subscribe((users: User[]) => {
      this.users = users;
    })
  }

}
