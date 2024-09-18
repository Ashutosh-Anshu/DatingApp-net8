import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes.service';
import { Member } from '../_models/member';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [ButtonsModule, FormsModule, MemberCardComponent,PaginationModule],
  templateUrl: './list.component.html',
  styleUrl: './list.component.css',
})
export class ListComponent implements OnInit, OnDestroy {
  
  likesService = inject(LikesService);
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadLikes();
  }

  getTitle() {
    switch (this.predicate) {
      case 'liked':
        return 'Members you liked';
      case 'likedBy':
        return 'Members who liked you';
      default:
        return 'Mutual';
    }
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate, this.pageNumber,this.pageSize);
  }
  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
  ngOnDestroy(): void {
    this.likesService.paginatedResult.set(null);
  }
}
