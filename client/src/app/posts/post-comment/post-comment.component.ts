import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, take } from 'rxjs';
import { QueryParameters } from 'src/app/helpers/queryParameters';
import { PostComment } from 'src/app/interfaces/postComment';
import { PostCommentResult } from 'src/app/interfaces/postCommentResult';
import { PostCommentService } from '../post-comment.service';


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
  commentsQueryParam = new QueryParameters();
  numberOfCommentsFetched = 0;
  commentsCount = 0;
  commentsFetched = false;
  loadMoreText = "Load more previous comments...";
  cursor = true;

  commentForm = new FormGroup({
    content: new FormControl('', [Validators.required])
  })

  constructor(public postCommentService: PostCommentService) { }

  ngOnInit(): void {
    // this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
    //   this.comments$.next([...this.commentsFetchedWithPost].reverse());
    // })
    this.loadComments();
  }

  onCommentsButtonClick() {
    this.hideComments = !this.hideComments;
    this.loadComments();
  }


  onCommentFormSubmit() {
    if (this.commentForm.invalid) {
      return;
    }

    this.hideComments = false;
    this.postCommentService.createComment({ content: this.commentForm.get('content').value }, this.postId).subscribe((response: PostComment) => {
      console.log(response);
      this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
        this.comments$.next([...comments, response]);
      })
      this.commentForm.reset();
    })

  }

  loadComments() {
    if (this.commentsFetched && this.commentsCount <= this.numberOfCommentsFetched) {
      this.loadMoreText = "No more previous comments..."
      this.cursor = false;

      return;
    }
    this.postCommentService.getComments(this.commentsQueryParam, this.postId).subscribe((result: PostCommentResult) => {
      this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
        console.log(result.comments);
        
        this.comments$.next([...(result.comments.reverse()), ...comments]);
        this.commentsQueryParam.pageNumber += 1;
      })
    })

  }

}
