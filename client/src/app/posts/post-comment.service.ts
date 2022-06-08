import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PostCommentService {
  rootUrl = 'https://localhost:5001/api/postcomments';

  constructor(private http: HttpClient) { }

  createPost(content: string, postId: number) {
    this.http.post(`${this.rootUrl}/posts/${postId}`, { content })
  }
}
