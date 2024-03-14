import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  userId: string = "";
  userFullName: string = "";
  username: string = "";


  constructor(
    private router: Router
  ) {

   }

   checkAuthentication(): boolean{
    try {
      const accessToken = localStorage.getItem("token");
      if (!accessToken) {
        this.handleAuthenticationFailure();
        return false;
      }
      const decodedToken: any = jwtDecode(accessToken);
      if (!decodedToken) {
        this.handleAuthenticationFailure();
        return false;
      }
      const currentTime = Date.now() / 1000; // saniye türünden şimdiki zaman
      if (decodedToken.exp < currentTime) { // token expire check
        this.handleAuthenticationFailure();
        return false;
      }
      this.userId = decodedToken.userId;
      this.userFullName = decodedToken.userFullName;
      this.username = decodedToken.username;
      if (!this.userId) {
        this.handleAuthenticationFailure();
        return false;
      }
      return true;

    } catch (error) {
      this.handleAuthenticationFailure();
      return false;
    }
    
   }

   handleAuthenticationFailure(){
    this.clearUserData();
    this.router.navigateByUrl("/login");
  }

   clearUserData(){
    this.userId = "";
    this.userFullName = "";
    this.username = "";
   }
}