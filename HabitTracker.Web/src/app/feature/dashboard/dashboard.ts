import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Slider } from '../../shared/components/slider/slider';
import { HabitService } from '../../core/services/habit.service';
import { CalenderView } from '../../shared/components/calender-view/calender-view';
import { Check, Circle, LucideAngularModule } from 'lucide-angular';
import { HabitCompletionService } from '../../core/services/habit.completion.service';
import { ModelView } from '../../shared/components/model-view/model-view';
import { AddHabit } from '../add-habit/add-habit';
import { UpdateHabitComponent } from '../update-habit/update-habit';
import { SnackbarService } from '../../core/services/snackbar.service';
import { UserService } from '../../core/services/user.service';

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

  constructor(
    private habitService: HabitService,
    private habitCompletionService: HabitCompletionService,
    private snackBar: SnackbarService,
    private userService: UserService
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
    this.userService.getHabits().subscribe({
      next: (res) => {
        this.habits = res?.data?.habits;
        this.getTotalHabitCount();
        if (this.habits.length > 0) {
          this.selectHabit(this.habits[0]);
        }
        this.loadStreakCounts();
      },
    });
  }

  getTotalHabitCount() {
    this.habitService.getTotalHabits().subscribe({
      next: (totalRes) => {
        this.summary.totalHabits = totalRes.data;
      },
      error: (err) => {
        console.error('Failed to fetch total habits:', err);
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
      next: () => {
        this.snackBar.showSuccess('Habit created successfully');
        this.loadHabits();
        this.showCreateForm = false;
      },
      error: (err) => {
        console.error('Failed to create habit:', err);
      },
    });
  }

  //  getOverallStreak(): number {
  //   if (!this.habits.length) return 0;

  //   const dailyHabits = this.habits.filter(
  //     (h) => h.frequency.toLowerCase() === 'daily'
  //   );
  //   if (!dailyHabits.length) return 0;

  //   let streak = 0;
  //   let dayOffset = 0;

  //   while (true) {
  //     const date = new Date();
  //     date.setDate(date.getDate() - dayOffset);
  //     const dateStr = date.toISOString().slice(0, 10);
  //     const todayStr = new Date().toISOString().slice(0, 10);
  //     if (dateStr > todayStr) break;

  //     // Only consider habits that have started on or before this date
  //     const habitsStarted = dailyHabits.filter(habit => {
  //       if (!habit.startDate) return true;
  //       return habit.startDate.slice(0, 10) <= dateStr;
  //     });

  //     if (!habitsStarted.length) {
  //       dayOffset++;
  //       continue;
  //     }

  //     const allCompleted = habitsStarted.every((habit) =>
  //       (habit.completions || []).some(
  //         (c: any) =>
  //           c.dateCompleted && c.dateCompleted.slice(0, 10) === dateStr
  //       )
  //     );

  //     if (allCompleted) {
  //       streak++;
  //       dayOffset++;
  //     } else {
  //       break;
  //     }
  //   }

  //   return streak;
  // }

  isFutureHabit(habit: any): boolean {
    if (!habit.startDate) return false;
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const habitDate = new Date(habit.startDate);
    habitDate.setHours(0, 0, 0, 0);
    return habitDate > today;
  }

  canCompleteCustomHabit(habit: any): boolean {
    if (habit.frequency.toLowerCase() !== 'custom' || !habit.customFrequency)
      return true;

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

  onHabitUpdated(updatedHabit: any) {
    const idx = this.habits.findIndex((h) => h.id === updatedHabit.id);
    if (idx !== -1) {
      this.habits[idx] = { ...this.habits[idx], ...updatedHabit };
      if (this.selectedHabit?.id === updatedHabit.id) {
        this.selectedHabit = this.habits[idx];
      }
    }
    this.onCloseUpdateModel();
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
