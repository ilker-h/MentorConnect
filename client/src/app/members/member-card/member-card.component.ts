import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member | undefined;

  // To switch between the Follow/Unfollow button
  membersUserSentConnectionRequestsTo: Member[] | undefined;
  predicate = 'connection_requested_from';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;
  isUserFollowed: boolean = false;

  constructor(private memberService: MembersService, private toastr: ToastrService, public presenceService: PresenceService) { }

  ngOnInit(): void {
    this.loadConnectionRequests();
  }

  addConnectionRequest(member: Member) {
    this.memberService.addConnectionRequest(member.userName).subscribe({
      next: () => this.toastr.success('You have followed ' + member.knownAs)
    });
    this.isUserFollowed = true;
  }

  removeConnectionRequest(member: Member) {
    this.memberService.removeConnectionRequest(member.userName).subscribe({
      next: () => this.toastr.info('You have unfollowed ' + member.knownAs)
    });
    this.isUserFollowed = false;
  }

  // To switch between the Follow/Unfollow button
  loadConnectionRequests() {
    this.memberService.getConnectionRequests(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.membersUserSentConnectionRequestsTo = response.result;
        this.pagination = response.pagination;

        this.membersUserSentConnectionRequestsTo?.forEach(element => {
          if (element.userName === this.member?.userName) {
            this.isUserFollowed = true;
          }
        });
      }
    });
  }

}
