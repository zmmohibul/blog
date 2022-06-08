import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService, User } from 'src/app/auth/auth.service';
import { PostQueryParameters } from 'src/app/helpers/postQueryParameter';
import { PagedResult } from 'src/app/interfaces/pagedResult';
import { Post } from 'src/app/interfaces/post';
import { PostService } from '../post.service';

@Component({
  selector: 'app-posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss']
})
export class PostsHomeComponent implements OnInit {
  posts: Post[] = [];
  result: PagedResult<Post>;
  postQueryParameters = new PostQueryParameters();
  hideComments = true;

  commentForm = new FormGroup({
    content: new FormControl('', [Validators.required])
  })

  constructor(private authService: AuthService, private postService: PostService) { }

  ngOnInit(): void {
    this.postService.fetchNextPosts().subscribe((posts: Post[]) => {
      this.posts.push(...posts);
    });
  }

  onScrollDown() {    
    this.postService.fetchNextPosts().subscribe((posts: Post[]) => {
      if (this.posts.length < this.postService.totalNumberOfPosts) {
        this.posts.push(...posts);
      }
    });
  }

  onCommentsButtonClick() {
    this.hideComments = !this.hideComments;
    console.log(this.hideComments);
    
    
  }

  onScrollUp() {

  }

  onCommentFormSubmit(postId: number) {
    if (this.commentForm.invalid) {
      return;
    }

    this.postService.createPost({ content: this.commentForm.get('content').value }, postId).subscribe(response => {
      console.log(response);
    })
  }
}
