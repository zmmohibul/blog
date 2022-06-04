import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, tap } from 'rxjs';

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

  getAllPosts() {
    if (this.allPosts.length === 0) {
      return this.http.get<Post[]>(`${this.rootUrl}`)
        .pipe(
          tap((posts: Post[]) => {
            this.allPosts = posts;
          }
        ));
    }
    
    return of(this.allPosts);
  }
}
