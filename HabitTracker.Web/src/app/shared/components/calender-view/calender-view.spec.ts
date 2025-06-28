import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalenderView } from './calender-view';

describe('CalenderView', () => {
  let component: CalenderView;
  let fixture: ComponentFixture<CalenderView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalenderView]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalenderView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
