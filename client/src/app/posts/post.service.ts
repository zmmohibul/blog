import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, of, take, tap } from 'rxjs';
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
  posts$ = new BehaviorSubject<Post[]>([]);
  totalNumberOfPosts = 0;
  numbersOfPostsFetched = 0;
  postFetched = false;
  postQueryParameter = new PostQueryParameters();

  constructor(private http: HttpClient) { }

  fetchNextPosts() {
    let numberOfPostsAfterFetching = this.postQueryParameter.pageNumber * this.postQueryParameter.pageSize;

    if (this.postFetched && this.numbersOfPostsFetched >= this.totalNumberOfPosts){
      return this.posts$;
    }

    return this.http
      .get<PagedResult<Post>>(`${this.rootUrl}?pageNumber=${this.postQueryParameter.pageNumber}&pageSize=${this.postQueryParameter.pageSize}`)
        .pipe(
          map((pagedResult: PagedResult<Post>) => {
            this.totalNumberOfPosts = pagedResult.count;
            this.numbersOfPostsFetched += pagedResult.data.length;

            this.posts$.pipe(take(1)).subscribe((posts: []) => {
              this.posts$.next([...posts, ...pagedResult.data]);
            });

            if (this.numbersOfPostsFetched < this.totalNumberOfPosts){
              this.postQueryParameter.pageNumber += 1;
            }

            this.postFetched = true;

            return pagedResult.data;
          })
        );
  }

  createPost(comment: CreateCommentDto, postId: number) {
    return this.http.post(`${this.commentRootUrl}/post/${postId}`, comment);
  }
}
