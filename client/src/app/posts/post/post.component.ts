import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Post } from 'src/app/interfaces/post';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  @Input() post: Post;
  @Output() commentsClick = new EventEmitter<boolean>();
  hideComments = true;
  constructor() { }

  ngOnInit(): void {
  }

  onCommentsButtonClick() {
    this.commentsClick.emit(!this.hideComments);
    this.hideComments = !this.hideComments;
  }
}
