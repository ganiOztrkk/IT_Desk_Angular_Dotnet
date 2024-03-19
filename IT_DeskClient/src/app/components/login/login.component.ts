import { Component, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { PasswordModule } from 'primeng/password';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { LoginModel } from '../../models/login.model';
import { InputSwitchModule } from 'primeng/inputswitch';
import { Router } from '@angular/router';
import { GoogleSigninButtonModule, SocialAuthService } from "@abacritt/angularx-social-login";
import { GoogleLoginModel } from '../../models/googleLogin.model';
import { ErrorService } from '../../services/error.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CardModule,
    ButtonModule,
    InputGroupModule,
    InputGroupAddonModule,
    PasswordModule,
    FormsModule,
    DividerModule,
    InputTextModule,
    ToastModule,
    InputSwitchModule,
    GoogleSigninButtonModule
  ],
  providers:[
    MessageService
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export default class LoginComponent implements OnInit {
  loginModel: LoginModel = new LoginModel();
  googleLoginModel : GoogleLoginModel = new GoogleLoginModel();
  password: string = "1234Aa";
  usernameOrEmail: string = "developer";
  hasRememberMe: boolean = false;

constructor(
  private messageService: MessageService,
  private http: HttpClient,
  private router: Router,
  private authService: SocialAuthService,
  private error: ErrorService
) {}

ngOnInit() {
  this.authService.authState.subscribe((user) => {
    this.googleLoginModel.id = user.id;
    this.googleLoginModel.idToken = user.idToken;
    this.googleLoginModel.firstName = user.firstName;
    this.googleLoginModel.lastName = user.lastName;
    this.googleLoginModel.name = user.name;
    this.googleLoginModel.photoUrl = user.photoUrl;
    this.googleLoginModel.email = user.email;
    this.http.post("http://localhost:5180/api/Auth/GoogleLogin", this.googleLoginModel)
    .subscribe({
      next:(res : any) => {
        localStorage.setItem("token", JSON.stringify(res.accessToken));
        this.router.navigateByUrl("/");
      },
      error: (err : HttpErrorResponse)=> {
        this.messageService.add({
          severity: 'error', 
          summary: 'Hata!', 
          detail: err.error.message
         });
      }
    })

  });
}

usernameOrEmailValidation(){
  let input = document.getElementById("usernameOrEmail");
  if (this.usernameOrEmail.length < 3) {
    input?.classList.add("ng-invalid", "ng-dirty")
  }else{
    input?.classList.remove("ng-invalid", "ng-dirty")
  }
}

passwordValidation(){
  let hasUppercase = /[A-Z]/.test(this.password);
  let hasLowercase = /[a-z]/.test(this.password);
  let hasNumber = /\d/.test(this.password);
  let passwordLength = this.password.length;

  if (hasUppercase) {
    document.getElementById("uppercase")?.classList.add("uppercaseValid")
  }else{
    document.getElementById("uppercase")?.classList.remove("uppercaseValid")
  }
  if (hasLowercase) {
    document.getElementById("lowercase")?.classList.add("lowercaseValid")
  }else{
    document.getElementById("lowercase")?.classList.remove("lowercaseValid")
  }
  if (hasNumber) {
    document.getElementById("numeric")?.classList.add("numericValid")
  }else{
    document.getElementById("numeric")?.classList.remove("numericValid")
  }
  if (passwordLength >= 6) {
    document.getElementById("charLength")?.classList.add("charLengthValid")
  }else{
    document.getElementById("charLength")?.classList.remove("charLengthValid")
  }

  if (hasLowercase && hasUppercase && hasNumber && passwordLength >= 6) {
    document.getElementById("password")?.classList.remove("ng-invalid", "ng-dirty");
  }else{
    document.getElementById("password")?.classList.add("ng-invalid", "ng-dirty");
  }
}

signIn(){
  let userInput = document.getElementById("usernameOrEmail")?.classList.contains("ng-invalid")
  let passwordInput = document.getElementById("password")?.classList.contains("ng-invalid");

  if (this.password == "" || this.usernameOrEmail == "" || userInput || passwordInput ) {
    this.messageService.add({
      severity: 'warn', 
      summary: 'Lütfen geçerli bilgi giriniz', 
      detail: 'Kullanıcı adı en az 3 karakter içermelidir ve şifre belirtilen kurallara uygun olmalıdır.' 
     });
  }else{
    this.loginModel.password = this.password;
    this.loginModel.usernameOrEmail = this.usernameOrEmail;
    this.loginModel.hasRememberMe = this.hasRememberMe;

    this.http.post("http://localhost:5180/api/Auth/Login", this.loginModel)
    .subscribe({
      next:(res : any) => {
        localStorage.setItem("token", JSON.stringify(res.accessToken));
        this.router.navigateByUrl("/");
      },
      error: (err : HttpErrorResponse)=> {
        this.error.errorHandler(err);
      }
    })
  }
}

}