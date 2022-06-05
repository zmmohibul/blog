import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostsRoutingModule } from './posts-routing.module';
import { PostsHomeComponent } from './posts-home/posts-home.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';



@NgModule({
  declarations: [
    PostsHomeComponent
  ],
  imports: [
    CommonModule,
    PostsRoutingModule,
    InfiniteScrollModule
  ]
})
export class PostsModule { }
