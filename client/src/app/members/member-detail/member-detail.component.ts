import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { TimeagoModule } from "ngx-timeago";
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, MemberMessagesComponent]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  // The template is considered a child of this ts component, so the reference variables in the template can be accessed with @ViewChild.
  // @ViewChild is dynamic by default, so it will wait until tabs are available before assigning the memberTabs variable.
  // To change this, adding {static: true} makes it so that memberTabs is constructed immediately.
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent; // {static: true}
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute,
    private messageService: MessageService, public presenceService: PresenceService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    });
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data['member'] // 'member' is the property name given in app-routing.module - {member: memberDetailedResolver}
    });

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab']) // tab is the key in {tab: 'Messages'}
      }
    });

    this.getImages();
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      });
    }
  }

  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get('username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: member => {
  //       this.member = member
  //     }
  //   });
  // }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading == 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true; // ! turns off typescript's typesafety
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  getImages() {
    if (!this.member) return;
    for (const photo of this.member?.photos) {
      this.images.push(new ImageItem({ src: photo.url, thumb: photo.url }));
    }
  }

}
