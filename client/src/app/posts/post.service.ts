import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, tap } from 'rxjs';
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
  allPosts: Post[] = [];

  constructor(private http: HttpClient) { }

  getAllPosts(queryParameters: QueryParameters) {
    return this.http.get<PagedResult<Post>>(`${this.rootUrl}?pageNumber=${queryParameters.pageNumber}&pageSize=${queryParameters.pageSize}`)
        .pipe(
          tap((pagedResult: PagedResult<Post>) => {
            this.allPosts = pagedResult.data;
          })
        );
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
