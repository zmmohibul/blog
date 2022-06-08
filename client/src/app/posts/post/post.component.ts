import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { PresenceService } from 'src/app/auth/presence.service';
import { Post } from 'src/app/interfaces/post';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  @Input() post: Post;
  hideComments = true;
  constructor(public presenceService: PresenceService) { }

  ngOnInit(): void {
  }

  onCommentsButtonClick() {
    this.hideComments = !this.hideComments;
  }
}
