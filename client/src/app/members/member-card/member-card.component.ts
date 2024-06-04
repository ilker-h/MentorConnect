import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(private memberService: MembersService, private toastr: ToastrService) { }

  addConnectionRequest(member: Member) {
    this.memberService.addConnectionRequest(member.userName).subscribe({
      next: () => this.toastr.success('You have sent a connection request to ' + member.knownAs)
    });
  }

}
