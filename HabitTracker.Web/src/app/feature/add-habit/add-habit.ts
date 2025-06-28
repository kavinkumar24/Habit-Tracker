import { Component, EventEmitter, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-add-habit',
  imports: [ReactiveFormsModule],
  templateUrl: './add-habit.html',
  styleUrl: './add-habit.css',
})
export class AddHabit {
  @Output() habitCreated = new EventEmitter<any>();

  habitForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.habitForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(50)]],
      frequency: ['daily', Validators.required],
      description: ['', [Validators.maxLength(200)]],
      startDate: ['', Validators.required],
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

  onSubmit() {
    if (this.habitForm.valid) {
      this.habitCreated.emit(this.habitForm.getRawValue());
      this.habitForm.reset({ frequency: 'daily' });
    }
  }
}
