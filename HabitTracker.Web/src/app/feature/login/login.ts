import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Spinner } from '../../shared/components/spinner/spinner';
import { UserLogin } from '../../core/models/UserLogin';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, Spinner],
  templateUrl: './login.html',
})
export class Login implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private snackBar: SnackbarService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      emailorUserName: ['', [Validators.required]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const usernameOrEmail = this.loginForm.value.emailorUserName;
      const password = this.loginForm.value.password;
      const userLogin: UserLogin = {
        usernameOrEmail: this.loginForm.value.emailorUserName,
        password: this.loginForm.value.password,
      };
      this.userService.login(userLogin).subscribe({
        next: (res) => {
          setTimeout(() => {
            this.isLoading = false;
            if (res?.success) {
              const { password, ...userWithoutPassword } = res.data || {};
              localStorage.setItem('user', JSON.stringify(userWithoutPassword));
              this.router.navigate(['/user/dashboard']);
            } else {
              this.snackBar.showInfo('Invalid Credientials');
            }
          }, 1000);
        },
        error: () => {
          this.isLoading = false;
          this.snackBar.showError('Failed to login!');
        },
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
