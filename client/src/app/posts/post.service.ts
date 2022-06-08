import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, of, tap } from 'rxjs';
import { PostQueryParameters } from '../helpers/postQueryParameter';
import { PagedResult } from '../interfaces/pagedResult';
import { Post } from '../interfaces/post';
import { PostComment } from '../interfaces/postComment';

interface CreateCommentDto {
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class PostService {
  rootUrl = 'https://localhost:5001/api/posts';
  commentRootUrl = 'https://localhost:5001/api/postcomments';
  posts: Post[] = [];
  public totalNumberOfPosts = 0;
  postQueryParameter = new PostQueryParameters();

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

  createPost(comment: CreateCommentDto, postId: number) {
    return this.http.post(`${this.commentRootUrl}/post/${postId}`, comment)
      .pipe(tap((comment: PostComment) => {
        this.posts.forEach((post => {
          if (post.id === postId) {
            post.comments.push(comment);
          }
        }))
      }))
  }
}
