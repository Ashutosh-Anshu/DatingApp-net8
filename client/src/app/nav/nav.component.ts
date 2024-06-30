import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, RouterLink, RouterLinkActive,TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toster = inject(ToastrService);
  model: any = {}
  login() {
    this.accountService.login(this.model).subscribe({
      next: () =>
        this.router.navigateByUrl('/members')
      ,
      error: error => this.toster.error(error.error)
    })
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
