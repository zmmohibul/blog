import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "posts",
    canLoad: [AuthGuard],
    loadChildren: () => import('./posts/posts.module').then(mod => mod.PostsModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
