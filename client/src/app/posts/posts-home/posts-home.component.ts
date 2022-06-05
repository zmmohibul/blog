import { Component, OnInit } from '@angular/core';
import { AuthService, User } from 'src/app/auth/auth.service';
import { QueryParameters } from 'src/app/helpers/queryParameters';
import { PagedResult } from 'src/app/interfaces/pagedResult';
import { Post, PostService } from '../post.service';

@Component({
  selector: 'app-posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss']
})
export class PostsHomeComponent implements OnInit {
  posts: Post[] = [];
  result: PagedResult<Post>;
  numbers: number[] = [];
  currentPage = 1;
  totalPages = 1;
  queryParameters = new QueryParameters();

  constructor(private authService: AuthService, private postService: PostService) { }

  ngOnInit(): void {
    const queryParameters = new QueryParameters();
    queryParameters.pageNumber = 1;
    queryParameters.pageSize = 2;

    this.postService.getAllPosts(queryParameters).subscribe((pagedResult: PagedResult<Post>) => {
      this.posts = pagedResult.data;
      this.result = pagedResult;
      this.totalPages = Math.ceil(pagedResult.count / pagedResult.pageSize);  
      this.numbers = Array(this.totalPages).fill(1).map((x,i)=>i+1);
      console.log(this.posts);
    })
    
  }

  onScrollDown() {    
    if (this.result.count > this.queryParameters.pageNumber * this.queryParameters.pageSize) {
      this.queryParameters.pageNumber += 1;
      this.queryParameters.pageSize = 2;

      this.postService.getAllPosts(this.queryParameters).subscribe((pagedResult: PagedResult<Post>) => {
        this.posts.push(...pagedResult.data);
        this.result = pagedResult;
        console.log(this.posts);
      })
    }
  }

  onScrollUp() {

  }
}
