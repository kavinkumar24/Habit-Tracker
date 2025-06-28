import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { HabitService } from '../../core/services/habit.service';
import { Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-update-habit',
  templateUrl: './update-habit.html',
  imports: [ReactiveFormsModule, FormsModule],
})
export class UpdateHabitComponent implements OnChanges {
  @Input() habit: any;
  @Output() close = new EventEmitter<void>();

  habitForm: FormGroup;

  constructor(
    private habitService: HabitService,
    private fb: FormBuilder,
    private snackBar: SnackbarService
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

    const payload: any = {};
    const formValue = this.habitForm.getRawValue();

    Object.keys(formValue).forEach((key) => {
      console.log(
        `Comparing key: ${key}, formValue: ${formValue[key]}, habit: ${this.habit[key]}`
      );

      if (key === 'customFrequency') {
        if (
          formValue['frequency'] === 'custom' &&
          formValue[key] !== this.habit[key]
        ) {
          payload[key] = formValue[key];
        }
        return;
      }

      if (key === 'startDate' && formValue[key] !== this.habit[key]) {
        payload[key] = new Date(formValue[key]).toISOString();
        return;
      }

      if (formValue[key] !== this.habit[key]) {
        payload[key] = this.habit[key];
      }
    });

    console.log('Update payload:', payload);

    this.habitService.updateHabit(this.habit.id, payload).subscribe({
      next: (res) => {
        console.log(res);
        this.snackBar.showSuccess("Habit updated");
        this.close.emit();
      },
      error: (err) => {
        console.error('Update failed', err);
      },
    });
  }
}
