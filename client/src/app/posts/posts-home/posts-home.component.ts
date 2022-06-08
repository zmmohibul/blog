import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService, User } from 'src/app/auth/auth.service';
import { PostService } from '../post.service';

@Component({
  selector: 'app-posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss']
})
export class PostsHomeComponent implements OnInit {
  hideComments = true;

  commentForm = new FormGroup({
    content: new FormControl('', [Validators.required])
  })

  constructor(private authService: AuthService, public postService: PostService) { }

  ngOnInit(): void {
    this.postService.fetchNextPosts().subscribe(() => {});
  }

  onScrollDown() {    
    this.postService.fetchNextPosts().subscribe(() => {});
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
