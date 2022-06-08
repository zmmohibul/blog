import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, take } from 'rxjs';
import { PostComment } from 'src/app/interfaces/postComment';
import { PostService } from '../post.service';

@Component({
  selector: 'app-post-comment',
  templateUrl: './post-comment.component.html',
  styleUrls: ['./post-comment.component.scss']
})
export class PostCommentComponent implements OnInit {
  @Input() postId: number;
  @Input() commentsFetchedWithPost: PostComment[];
  comments$ = new BehaviorSubject<PostComment[]>([]);
  @Input() hideComments = true;

  commentForm = new FormGroup({
    content: new FormControl('', [Validators.required])
  })

  constructor(public postService: PostService) { }

  ngOnInit(): void {
    this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
      this.comments$.next([...this.commentsFetchedWithPost]);
    })
  }

  onCommentsButtonClick() {
    this.hideComments = !this.hideComments;
  }


  onCommentFormSubmit() {
    if (this.commentForm.invalid) {
      return;
    }

    this.hideComments = false;
    this.postService.createPost({ content: this.commentForm.get('content').value }, this.postId).subscribe((response: PostComment) => {
      console.log(response);
      this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
        this.comments$.next([...comments, response]);
      })
      this.commentForm.reset();
    })

  }

}
