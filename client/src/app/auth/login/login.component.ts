import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, LoginCredentials } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    const loginCredentials: LoginCredentials = {
      username: this.loginForm.get('username').value,
      password: this.loginForm.get('password').value
    };

    this.authService.login(loginCredentials).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl("/posts")
      },
      error: (error) => {
        console.log(error.error);
        if (error.error.statusCode === 401) {
          this.loginForm.setErrors({ 'invalidCredential': true })
        } else {
          this.loginForm.setErrors({ 'somethingWentWrong': true })
        }
      }
    })
  }

}
