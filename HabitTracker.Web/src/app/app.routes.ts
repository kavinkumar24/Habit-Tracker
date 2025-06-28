import { Routes } from '@angular/router';
import { Login } from './feature/login/login';
import { Register } from './feature/register/register';
import { LoginGuard } from './core/guard/login-guard';
import { Dashboard } from './feature/dashboard/dashboard';
import { AuthGuard } from './core/guard/auth-guard';
import { LayoutComponent } from './feature/layout/layout-component/layout-component';

export const routes: Routes = [
    
    {path:'', redirectTo:'/login', pathMatch:'full'},
    {path:'login', component:Login, canActivate:[LoginGuard]},
    {path:'register', component:Register, canActivate:[LoginGuard]},
    {path:'user', component:LayoutComponent, canActivate:[AuthGuard],
        children:[
            {path:'', redirectTo:'dashboard', pathMatch:'full'},
            {path:'dashboard', component:Dashboard},
        ]
    }
];
