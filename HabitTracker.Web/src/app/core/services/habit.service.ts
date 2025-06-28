import { Injectable } from '@angular/core';
import { environment } from '../../../env/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HabitService {
  private baseUrl = `${environment.apiUrl}/Habit`;

  constructor(private http: HttpClient) {}

  getHabits(): Observable<{ data: { habits: any[] } }> {
    const user = localStorage.getItem('user');
    const userId = user ? JSON.parse(user).id : null;
    return this.http.get<{ data: { habits: any[] } }>(
      `${environment.apiUrl}/User/${userId}/habits`
    );
  }

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

  removeCompletion(habitId: string, dateCompleted: string): Observable<any> {
    const body = { habitId, dateCompleted };
    return this.http.request(
      'delete',
      `${environment.apiUrl}/HabitCompletion/remove`,
      { body }
    );
  }

  createHabit(habit:any):Observable<any>{
    return this.http.post(`${this.baseUrl}`, habit);
  }
}
