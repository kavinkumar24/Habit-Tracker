import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateHabit } from './update-habit';

describe('UpdateHabit', () => {
  let component: UpdateHabit;
  let fixture: ComponentFixture<UpdateHabit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateHabit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateHabit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
