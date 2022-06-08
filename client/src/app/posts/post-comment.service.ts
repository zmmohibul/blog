import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QueryParameters } from '../helpers/queryParameters';

interface CreateCommentDto {
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class PostCommentService {
  rootUrl = 'https://localhost:5001/api/postcomments';

  constructor(private http: HttpClient) { }

  createComment(comment: CreateCommentDto, postId: number) {
    return this.http.post(`${this.rootUrl}/post/${postId}`, comment);
  }

  getComments(qm: QueryParameters, postId: number) {
    
    return this.http.get(`${this.rootUrl}/post/${postId}?pageNumber=${qm.pageNumber}&pageSize=${qm.pageSize}`);
  }
}
