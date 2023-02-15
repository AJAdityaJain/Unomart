import { Component } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent {
  Message:string|null = 'Somn';

  constructor() {
    this.Message = new URLSearchParams(window.location.search).get('m');
  }
}
