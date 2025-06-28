import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HomeIcon, LogOutIcon, LucideAngularModule, MenuIcon } from 'lucide-angular';


@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.html',
  standalone: true,
  imports: [LucideAngularModule, CommonModule]
})
export class Sidebar implements OnInit {
  readonly homeIcon = HomeIcon;
  readonly logoutIcon = LogOutIcon;
  readonly menuIcon = MenuIcon;
  userName = '';
  userInitial = '';
  sidebarOpen = false;


  constructor(private router: Router) {}
 
  ngOnInit() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    this.userName = user.username || user.email || 'User';
    this.userInitial = (this.userName[0] || 'U').toUpperCase();
  }

  logout() {
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}