import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Spinner } from "../../shared/components/spinner/spinner";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, Spinner],
  templateUrl: './register.html',
})
export class Register implements OnInit{
  registerForm!: FormGroup;
  isLoading = false;
  constructor(private formBuilder: FormBuilder,
    private userService:UserService,
    private snackBar: SnackbarService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.userService.register(this.registerForm.value).subscribe({
        next: (res) => {
          setTimeout(()=>{
            this.isLoading = false;
            if (res?.success) {
            this.snackBar.showSuccess('Registration successful! Please login.');
            this.router.navigate(['/login']);
          } else {
            this.snackBar.showError(res?.message || 'Registration failed.');
          }
          },1000)
        },
        error: () => {
          this.isLoading = false;
          this.snackBar.showError('Registration failed!');
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }
}