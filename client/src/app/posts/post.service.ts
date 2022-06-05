import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, tap } from 'rxjs';
import { PostQueryParameters } from '../helpers/postQueryParameter';
import { QueryParameters } from '../helpers/queryParameters';
import { PagedResult } from '../interfaces/pagedResult';

export interface Post {
  id: number;
  title: string;
  content: string;
  createdAt: Date;
  createdBy: string;
}

@Injectable({
  providedIn: 'root'
})
export class PostService {
  rootUrl = 'https://localhost:5001/api/posts';
  posts: Post[] = [];
  public totalNumberOfPosts = 0;
  postQueryParameter = new PostQueryParameters();
  allFetched = false;

  constructor(private http: HttpClient) { }

  fetchNextPosts() {
    let numberOfPostsAfterFetching = this.postQueryParameter.pageNumber * this.postQueryParameter.pageSize;

    if (this.posts.length >= numberOfPostsAfterFetching) {
      return of(this.posts);
    } else {
      return this.http.get<PagedResult<Post>>(`${this.rootUrl}?pageNumber=${this.postQueryParameter.pageNumber}&pageSize=${this.postQueryParameter.pageSize}`)
        .pipe(
          map((pagedResult: PagedResult<Post>) => {
            this.posts.push(...pagedResult.data);
            this.totalNumberOfPosts = pagedResult.count;
            if (Math.ceil(this.totalNumberOfPosts / this.postQueryParameter.pageNumber) > this.postQueryParameter.pageNumber) {
              this.postQueryParameter.pageNumber += 1;
            }
            return pagedResult.data;
          })
        );
    }
  }

  getAllPosts(queryParameters: PostQueryParameters) {
    let numberOfPostsAfterFetching = queryParameters.pageNumber * queryParameters.pageSize;
    if (this.posts.length >= numberOfPostsAfterFetching) {
      return of(this.posts);
    } else {
      return this.http.get<PagedResult<Post>>(`${this.rootUrl}?pageNumber=${queryParameters.pageNumber}&pageSize=${queryParameters.pageSize}`)
        .pipe(
          map((pagedResult: PagedResult<Post>) => {
            this.posts.push(...pagedResult.data);
            this.totalNumberOfPosts = pagedResult.count;
            return pagedResult.data;
          })
        );
    }

    // check if posts has already been fetched before making new get request
    // calculate total number of posts after making new get request
    // if the posts.length greater than that number then posts already been fetched
    // 


    // return this.http.get<PagedResult<Post>>(`${this.rootUrl}?pageNumber=${queryParameters.pageNumber}&pageSize=${queryParameters.pageSize}`)
    //     .pipe(
    //       tap((pagedResult: PagedResult<Post>) => {
    //         // this.allPosts = pagedResult.data;
    //       })
    //     );
    // if (this.allPosts.length === 0) {
    //   return this.http.get<PagedResult<Post>>(`${this.rootUrl}`)
    //     .pipe(
    //       tap((pagedResult: PagedResult<Post>) => {
    //         this.allPosts = pagedResult.data;
    //       })
    //     );
    // }
    
    // return of(this.allPosts);
  }
}
