import {
  Component,
  inject,
  input,
  OnInit,
  output,
  ViewChild,
} from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/messages';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent {
  private messageService = inject(MessageService);
  username = input.required<string>();
  messages = input.required<Message[]>();
  meesageContent = '';
  updateMessage = output<Message>();
  @ViewChild('messageForm') messageForm?: NgForm;

  sendMessage() {
    debugger
    this.messageService
      .sendMessage(this.username(), this.meesageContent)
      .subscribe({
        next: (message) => {
          this.updateMessage.emit(message);
          this.messageForm?.reset()
        },
      });
  }
}
