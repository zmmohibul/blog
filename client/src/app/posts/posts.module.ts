import { NgModule } from '@angular/core';

import { PostsRoutingModule } from './posts-routing.module';
import { PostsHomeComponent } from './posts-home/posts-home.component';
import { PostComponent } from './post/post.component';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { PostCommentComponent } from './post-comment/post-comment.component';



@NgModule({
  declarations: [
    PostsHomeComponent,
    PostComponent,
    PostCommentComponent,
  ],
  imports: [
    PostsRoutingModule,
    SharedModule,
    CommonModule,
    ReactiveFormsModule
  ]
})
export class PostsModule { }
