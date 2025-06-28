import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { HabitService } from '../../core/services/habit.service';
import { Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-update-habit',
  templateUrl: './update-habit.html',
  styleUrl: './update-habit.css',
  imports: [ReactiveFormsModule, FormsModule],
})
export class UpdateHabitComponent implements OnChanges {
  @Input() habit: any;

  habitForm: FormGroup;

  constructor(
    private habitService: HabitService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.habitForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(200)]],
      startDate: ['', Validators.required],
      frequency: ['daily', Validators.required],
      customFrequency: [{ value: '', disabled: true }, [Validators.min(1)]],
    });

    this.habitForm.get('frequency')?.valueChanges.subscribe((value) => {
      const customFreq = this.habitForm.get('customFrequency');
      if (value === 'custom') {
        customFreq?.enable();
        customFreq?.setValidators([Validators.required, Validators.min(1)]);
      } else {
        customFreq?.disable();
        customFreq?.clearValidators();
        customFreq?.setValue('');
      }
      customFreq?.updateValueAndValidity();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['habit'] && this.habit) {
      console.log('Patch value:', this.habit);
      this.habitForm.patchValue({
        title: this.habit.title,
        description: this.habit.description,
        startDate: this.habit.startDate
          ? typeof this.habit.startDate === 'string'
            ? this.habit.startDate.split('T')[0]
            : new Date(this.habit.startDate).toISOString().split('T')[0]
          : '',
        frequency: this.habit.frequency,
        customFrequency: this.habit.customFrequency,
      });
    }
  }

  updateHabit() {
    if (this.habitForm.invalid) return;

    const payload = {
      ...this.habit,
      ...this.habitForm.getRawValue(),
      customFrequency:
        this.habitForm.get('frequency')?.value === 'custom'
          ? this.habitForm.get('customFrequency')?.value
          : null,
    };

    this.habitService.updateHabit(this.habit.id, payload).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Update failed', err);
      },
    });
  }
}
