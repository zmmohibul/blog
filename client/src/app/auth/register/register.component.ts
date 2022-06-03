import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, RegisterCredentials } from '../auth.service';
import { MatchPassword } from '../validators/match-password';
import { UsernameExists } from '../validators/username-exists';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  authForm = new FormGroup({
    username: new FormControl(
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        Validators.pattern(/^[a-z0-9_]+$/)
      ],
      [
        this.usernameExists.validate
      ]
    ),
    password: new FormControl(
      '',
      [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(20),
        Validators.pattern(/^(?=(.*[a-z]){1,})(?=(.*[A-Z]){1,})(?=(.*[0-9]){1,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{6,}$/)
      ]
    ),
    passwordConfirmation: new FormControl(
      '',
      [
        Validators.required,
      ]
    )
  }, { validators: this.matchPassword.validate })

  constructor(private matchPassword: MatchPassword, private usernameExists: UsernameExists, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.authForm.invalid) {
      return;
    }

    const credential: RegisterCredentials = {
      username: this.authForm.get('username').value,
      password:  this.authForm.get('username').value
    }

    this.authService.register(credential).subscribe({
      next: response => {
        console.log(response);
        this.router.navigateByUrl("/posts");
      },
      error: err => {
        console.log(err);
      }
    });
  }


}
