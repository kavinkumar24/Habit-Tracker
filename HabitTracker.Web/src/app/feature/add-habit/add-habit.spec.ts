import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddHabit } from './add-habit';

describe('AddHabit', () => {
  let component: AddHabit;
  let fixture: ComponentFixture<AddHabit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddHabit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddHabit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
