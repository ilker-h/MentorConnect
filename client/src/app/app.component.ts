import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { HostListener } from "@angular/core";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'MentorConnect';
  screenHeight?: number;
  screenWidth?: number;

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
    this.onResize();
    // this.getScreenSize();
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
     this.screenHeight = window.innerHeight;
     this.screenWidth = window.innerWidth;
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }


}
