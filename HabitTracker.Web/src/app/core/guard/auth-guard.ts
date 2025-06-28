import { CanActivateFn } from '@angular/router';

export const AuthGuard: CanActivateFn = (route, state) => {
  
  if (!localStorage.getItem('user')) {
    window.location.href = '/login';
    return false;
  }
  return true;
};
