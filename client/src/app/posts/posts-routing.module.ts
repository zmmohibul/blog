import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { PostComponent } from './post/post.component';
import { PostsHomeComponent } from './posts-home/posts-home.component';

const routes: Routes = [
  { path: "", component: PostsHomeComponent },
  { path: "/post", component: PostComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostsRoutingModule { }
