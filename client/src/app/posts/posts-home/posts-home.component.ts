import { Component, OnInit } from '@angular/core';
import { AuthService, User } from 'src/app/auth/auth.service';
import { Post, PostService } from '../post.service';

@Component({
  selector: 'app-posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss']
})
export class PostsHomeComponent implements OnInit {
  posts: Post[] = [];

  constructor(private authService: AuthService, private postService: PostService) { }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe((posts: Post[]) => {
      console.log(posts);
      this.posts = posts;
    })
    
  }

}
