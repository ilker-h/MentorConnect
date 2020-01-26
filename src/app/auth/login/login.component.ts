import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public onLogin(form: NgForm) {
    const email = form.value.email;
    const password = form.value.password;
    // this.authService.loginUser(email, password);
  }

}
