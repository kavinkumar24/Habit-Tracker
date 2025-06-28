
import { Injectable } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const LoginGuard: CanActivateFn = (route, state) => {
  
  if (localStorage.getItem('user')) {
    window.location.href = '/user/dashboard'; 
    return false;
  }
  return true;
};