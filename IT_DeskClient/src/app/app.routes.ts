import { Routes } from '@angular/router';
import { LayoutsComponent } from './components/layouts/layouts.component';
import { inject } from '@angular/core';
import { AuthService } from './services/auth.service';

export const routes: Routes = [
    {
        path: "login",
        loadComponent: () => import ("./components/login/login.component") // component kısmı lazy load ile yüklenmesi için load component metodu ile girdik. .then() ile yakalamamak için ilgili componente default ekledik.
    },
    {
        path: "",
        component: LayoutsComponent,
        canActivateChild: [
            ()=> inject(AuthService).checkAuthentication() // metot true dönmediği sürece layout altındaki sayfalara erişilemez.
        ],
        children: [
            {
                path: "",
                loadComponent: () => import ("./components/home/home.component")
            }
        ]
    }
];