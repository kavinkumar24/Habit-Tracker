import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Slider } from '../../shared/components/slider/slider';
import { HabitService } from '../../core/services/habit.service';
import { CalenderView } from '../../shared/components/calender-view/calender-view';
import {
  Check,
  Circle,
  LucideAngularModule,
} from 'lucide-angular';
import { HabitCompletionService } from '../../core/services/habit.completion.service';
import { ModelView } from '../../shared/components/model-view/model-view';
import { AddHabit } from '../add-habit/add-habit';
import { UpdateHabitComponent } from '../update-habit/update-habit';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-dashboard',
  imports: [
    FormsModule,
    CommonModule,
    Slider,
    CalenderView,
    LucideAngularModule,
    NgOptimizedImage,
    ModelView,
    AddHabit,
    UpdateHabitComponent,
  ],
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  readonly checkIcon = Check;
  readonly circleIcon = Circle;
  summary = {
    totalHabits: 0,
    activeStreaks: 0,
  };

  selectedCategory = 'daily';
  completedPercent: number = 0;

  selectedHabit: any = null;
  showUpdateModel = false;
  showDelete = false;
  selectedHabitForUpdate: any;
  selectedHabitForDelete: any;
  showCreateForm = false;

  selectHabit(habit: any) {
    this.selectedHabit = habit;
    this.completions = (habit.completions || []).map((c: any) =>
      c.dateCompleted ? c.dateCompleted.slice(0, 10) : ''
    );
  }

  get filteredHabits() {
    return this.habits.filter(
      (habit) => habit.frequency.toLowerCase() === this.selectedCategory
    );
  }

  habits: any[] = [];
  completions: any[] = [];
  streakDates: string[] = [];

  constructor(
    private habitService: HabitService,
    private habitCompletionService: HabitCompletionService,
    private snackBar: SnackbarService
  ) {}
  ngOnInit(): void {
    this.loadHabits();
    this.loadCompletionPercent();
  }

  loadStreakCounts() {
    this.habits.forEach((habit) => {
      this.habitCompletionService.getStreakCount(habit.id).subscribe((res) => {
        habit.streakCount = res.data ?? 0;
      });
    });
  }

  loadHabits() {
    this.habitService.getHabits().subscribe({
      next: (res) => {
        this.habits = res?.data?.habits;
        this.summary.totalHabits = this.habits.length;
        this.summary.activeStreaks = this.areAllHabitsCompletedToday()
          ? this.habits.reduce((sum, h) => sum + (h.streakCount || 0), 0)
          : 0;
        if (this.habits.length > 0) {
          this.selectHabit(this.habits[0]);
        }
        this.loadStreakCounts();
      },
    });
  }

  deleteHabit(habitId: string) {
    this.habitService.deleteHabit(habitId).subscribe({
      next: () => {
        this.loadHabits();
        this.onCloseDeleteModel();
      },
      error: (err) => {
        console.error('Failed to delete habit:', err);
      },
    });
  }

  isCompletedToday(habit: any): boolean {
    const today = new Date().toISOString().slice(0, 10);
    const completions = habit.completions || [];
    return completions.some(
      (c: any) => c.dateCompleted && c.dateCompleted.slice(0, 10) === today
    );
  }

  toggleComplete(habit: any) {
    if (this.isCompletedToday(habit)) {
      const today = new Date().toISOString().slice(0, 10);
      const completion = habit.completions.find(
        (c: any) => c.dateCompleted && c.dateCompleted.slice(0, 10) === today
      );
      if (completion) {
        this.habitCompletionService
          .removeCompletion(habit.id, completion.dateCompleted)
          .subscribe(() => {
            this.loadHabits();
            this.loadCompletionPercent();
          });
      }
    } else {
      this.markComplete(habit.id);
    }
  }

  markComplete(habitId: string) {
    this.habitService.markCompleted(habitId).subscribe(() => {
      this.loadHabits();
      this.loadCompletionPercent();
    });
  }

  loadCompletionPercent() {
    const user = localStorage.getItem('user');
    const userId = user ? JSON.parse(user).id : null;
    const today = new Date().toISOString();
    if (userId) {
      this.habitCompletionService
        .getCompletionPercentage(userId, today)
        .subscribe((res) => {
          this.completedPercent = Math.round(res.data);
        });
    }
  }

  newHabit = {
    title: '',
    description: '',
    frequency: 'daily',
    startDate: new Date().toISOString().split('T')[0],
  };

  addHabit(habit: any) {
    const user = localStorage.getItem('user');
    const userId = user ? JSON.parse(user).id : null;

    let startDate = habit.startDate;
    if (startDate) {
      const dateObj = new Date(startDate);
      startDate = new Date(
        Date.UTC(dateObj.getFullYear(), dateObj.getMonth(), dateObj.getDate())
      ).toISOString();
    }

    const payload: any = {
      ...habit,
      userId,
      startDate,
      customFrequency:
        habit.frequency === 'custom' ? habit.customFrequency : null,
    };

    this.habitService.createHabit(payload).subscribe({
      next: (res) => {
        this.snackBar.showSuccess('Habit created successfully');
        this.loadHabits();
        this.showCreateForm = false;
      },
      error: (err) => {
        console.error('Failed to create habit:', err);
      },
    });
  }

  getOverallStreak(): number {
    if (!this.habits.length) return 0;

    const dailyHabits = this.habits.filter(
      (h) => h.frequency.toLowerCase() === 'daily'
    );
    if (!dailyHabits.length) return 0;

    let streak = 0;
    let dayOffset = 0;

    while (true) {
      const date = new Date();
      date.setDate(date.getDate() - dayOffset);
      const dateStr = date.toISOString().slice(0, 10);

      const allCompleted = dailyHabits.every((habit) =>
        (habit.completions || []).some(
          (c: any) =>
            c.dateCompleted && c.dateCompleted.slice(0, 10) === dateStr
        )
      );

      if (allCompleted) {
        streak++;
        dayOffset++;
      } else {
        break;
      }
    }

    return streak;
  }

  isFutureHabit(habit: any): boolean {
    if (!habit.startDate) return false;
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const habitDate = new Date(habit.startDate);
    habitDate.setHours(0, 0, 0, 0);
    return habitDate > today;
  }

  areAllHabitsCompletedToday(): boolean {
    if (!this.habits.length) return false;
    const today = new Date().toISOString().slice(0, 10);
    return this.habits.every((habit) =>
      (habit.completions || []).some(
        (c: any) => c.dateCompleted && c.dateCompleted.slice(0, 10) === today
      )
    );
  }

  isCompletedThisWeek(habit: any): boolean {
    if (habit.frequency !== 'weekly' || !habit.completions) return false;
    const now = new Date();
    const startOfWeek = new Date(now);
    startOfWeek.setDate(now.getDate() - now.getDay());
    startOfWeek.setHours(0, 0, 0, 0);
    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(startOfWeek.getDate() + 6);
    endOfWeek.setHours(23, 59, 59, 999);

    return habit.completions.some((c: any) => {
      const completedDate = new Date(c.dateCompleted);
      return completedDate >= startOfWeek && completedDate <= endOfWeek;
    });
  }

  canCompleteCustomHabit(habit: any): boolean {
    if (habit.frequency !== 'custom' || !habit.customFrequency) return true;

    const completions = habit.completions || [];
    if (completions.length === 0) return true;

    const lastCompletion = completions
      .map((c: any) => new Date(c.dateCompleted))
      .sort((a: Date, b: Date) => b.getTime() - a.getTime())[0];
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const nextAllowedDate = new Date(lastCompletion);
    nextAllowedDate.setDate(nextAllowedDate.getDate() + habit.customFrequency);

    return today >= nextAllowedDate;
  }

  openUpdateModel(habit: any) {
    this.selectedHabitForUpdate = { ...habit };
    this.showUpdateModel = true;
  }

  openDeleteModel(habitId: any) {
    this.selectedHabitForDelete = habitId;
    this.showDelete = true;
  }

  onCloseDeleteModel() {
    this.showDelete = false;
    this.selectedHabitForDelete = null;
  }

  onCloseUpdateModel() {
    this.showUpdateModel = false;
    this.selectedHabitForUpdate = null;
  }
}
