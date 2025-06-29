import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-calender-view',
  imports: [CommonModule],
  templateUrl: './calender-view.html',
})
export class CalenderView implements OnChanges {
  @Input() completions: string[] = [];
  @Input() streaks: string[] = [];
  weeks: any[][] = [];
  currentMonth: number = new Date().getMonth();
  currentYear: number = new Date().getFullYear();

  ngOnInit() {
    this.generateCalendar();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['completions'] || changes['streaks']) {
      this.generateCalendar();
    }
  }

  generateCalendar() {
    const toLocalDateString = (date: string) => {
      const d = new Date(date);
      return `${d.getFullYear()}-${(d.getMonth() + 1)
        .toString()
        .padStart(2, '0')}-${d.getDate().toString().padStart(2, '0')}`;
    };

    const completions = Array.isArray(this.completions)
      ? this.completions.map(toLocalDateString)
      : [];
    const streaks = Array.isArray(this.streaks)
      ? this.streaks.map(toLocalDateString)
      : [];

    const firstDay = new Date(this.currentYear, this.currentMonth, 1);
    const startDay = firstDay.getDay();
    const daysInMonth = new Date(
      this.currentYear,
      this.currentMonth + 1,
      0
    ).getDate();
    const daysArray = [];

    for (let i = 1; i <= daysInMonth; i++) {
      const d = new Date(this.currentYear, this.currentMonth, i);
      const dateStr = `${d.getFullYear()}-${(d.getMonth() + 1)
        .toString()
        .padStart(2, '0')}-${d.getDate().toString().padStart(2, '0')}`;
      daysArray.push({
        day: i,
        completed: completions.includes(dateStr),
        streak: streaks.includes(dateStr),
      });
    }

    const padded = Array(startDay).fill(null).concat(daysArray);
    while (padded.length % 7 !== 0) padded.push(null);
    this.weeks = [];
    for (let i = 0; i < padded.length; i += 7) {
      this.weeks.push(padded.slice(i, i + 7));
    }
  }

  get monthName(): string {
    return new Date(this.currentYear, this.currentMonth).toLocaleString(
      'default',
      { month: 'long' }
    );
  }

  get year(): number {
    return this.currentYear;
  }

  prevMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendar();
  }

  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendar();
  }
}
