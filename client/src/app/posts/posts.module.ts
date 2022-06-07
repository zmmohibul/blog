import { NgModule } from '@angular/core';

import { PostsRoutingModule } from './posts-routing.module';
import { PostsHomeComponent } from './posts-home/posts-home.component';
import { PostComponent } from './post/post.component';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [
    PostsHomeComponent,
    PostComponent,
  ],
  imports: [
    PostsRoutingModule,
    SharedModule,
    CommonModule
  ]
})
export class PostsModule { }
