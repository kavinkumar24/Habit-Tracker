import { Injectable } from '@angular/core';
import { environment } from '../../../env/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HabitService {
  private baseUrl = `${environment.apiUrl}/Habit`;

  constructor(private http: HttpClient) {}

  markCompleted(habitId: string): Observable<any> {
    const body = {
      habitId,
      dateCompleted: new Date().toISOString(),
    };
    return this.http.post(
      `${environment.apiUrl}/HabitCompletion/complete`,
      body
    );
  }

  deleteHabit(habitId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${habitId}`);
  }

  updateHabit(habitId: string, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${habitId}`, data);
  }

  createHabit(habit:any):Observable<any>{
    return this.http.post(`${this.baseUrl}`, habit);
  }

  getTotalHabits(): Observable<any> {
    const user = localStorage.getItem('user');
    const userId = user ? JSON.parse(user).id : null;
    return this.http.get(`${this.baseUrl}/totalCount/${userId}`);
  }
}
