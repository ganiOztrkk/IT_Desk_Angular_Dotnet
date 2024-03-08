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
        this.router.navigateByUrl("/login");
        return false;
      }
      const decodedToken: any = jwtDecode(accessToken);
      if (!decodedToken) {
        this.router.navigateByUrl("/login");
        return false;
      }
      const currentTime = Date.now() / 1000; // saniye türünden şimdiki zaman
      if (decodedToken.exp < currentTime) { // token expire check
        this.clearUserData();
        this.router.navigateByUrl("/login");
        return false;
      }
      this.userId = decodedToken.userId;
      this.userFullName = decodedToken.userFullName;
      this.username = decodedToken.username;
      if (!this.userId) {
        this.clearUserData();
        this.router.navigateByUrl("/login");
        return false;
      }
      return true;

    } catch (error) {
      this.clearUserData(); // Clear user data on error
      this.router.navigateByUrl("/login");
      return false;
    }
    
   }

   clearUserData(){
    this.userId = "";
    this.userFullName = "";
    this.username = "";
   }
}