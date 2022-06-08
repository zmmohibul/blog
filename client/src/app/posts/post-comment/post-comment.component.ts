import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject, take } from 'rxjs';
import { PostComment } from 'src/app/interfaces/postComment';

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

  constructor() { }

  ngOnInit(): void {
    this.comments$.pipe(take(1)).subscribe((comments: PostComment[]) => {
      this.comments$.next([...this.commentsFetchedWithPost]);
    })
  }

  onCommentsButtonClick() {
    this.hideComments = !this.hideComments;
  }

}
