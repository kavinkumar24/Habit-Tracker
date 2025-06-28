import { Injectable } from "@angular/core";
import { environment } from "../../../env/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()

export class HabitCompletionService{
    private baseUrl = `${environment.apiUrl}`;
    constructor(private http: HttpClient) {}

    getCompletionPercentage(userId: string, date: string): Observable<{ data: number }> {
    const encodedDate = encodeURIComponent(date);
    return this.http.get<{ data: number }>(
        `${environment.apiUrl}/Habit/completion/${userId}/${encodedDate}`
    );
    }

    getStreakDates(habitId: string) {
        return this.http.get<{ data: string[] }>(`${environment.apiUrl}/Habit/streak/${habitId}`);
}

getStreakCount(habitId: string) {
  return this.http.get<{ data: number }>(`${environment.apiUrl}/Habit/streak/${habitId}`);
}
}