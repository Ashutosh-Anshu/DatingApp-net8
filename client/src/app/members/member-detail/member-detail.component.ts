import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/messages';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [
    TabsModule,
    GalleryModule,
    TimeagoModule,
    DatePipe,
    MemberMessagesComponent,
  ],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  private messageService = inject(MessageService);
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  private route = inject(ActivatedRoute);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

  ngOnInit(): void {
    debugger;
    this.route.data.subscribe({
      next: (data) => {
        this.member = data['member'];
        this.member &&
          this.member.photos.map((p) => {
            this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
          });
      },
    });

    this.route.queryParams.subscribe({
      next: (params) => {
        params['tab'] && this.selectTab(params['tab']);
      },
    });
  }
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (
      this.activeTab.heading == 'Messages' &&
      this.messages.length === 0 &&
      this.member
    ) {
      this.messageService.GetMessageThread(this.member.userName).subscribe({
        next: (message) => (this.messages = message),
      });
    }
  }
  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get('username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: (member) => {
  //       this.member = member;
  //       member.photos.map((p) => {
  //         this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
  //       });
  //     },
  //   });
  // }
  selectTab(heading: string) {
    if (this.memberTabs) {
      const memberTab = this.memberTabs.tabs.find((x) => x.heading === heading);
      if (memberTab) {
        this.memberTabs.tabs.forEach((tab) => (tab.active = false));
        memberTab.active = true;
      }
    }
  }

  onUpdateMessages(event:Message){
    debugger
    this.messages.push(event);
  }
}
