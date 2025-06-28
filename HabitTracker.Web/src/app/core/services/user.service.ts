import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../env/environment';

export interface User {
  id?: string;
  username: string;
  email: string;
  password: string;
  habits?: any;
}

@Injectable()
export class UserService {
  private apiUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  register(user: User): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  login(usernameOrEmail: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { usernameOrEmail, password });
  }

  getUserByEmail(email: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/email/${email}`);
  }

  getUserByUsername(username: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/username/${username}`);
  }
}