import { Component, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-num-input',
  templateUrl: './num-input.component.html',
  styleUrls: ['./num-input.component.scss']
})
export class NumInputComponent {
  @Input() Amount:string = '0';
  @Input() disabled:string = '';
  @Output() Update:EventEmitter<number> = new EventEmitter<number>();
  
  amount:number = 0;
  disable = false;

  constructor(){}
  ngOnInit(){
    if(this.disabled == 'true')
      this.disable = true;
    this.amount = Number.parseInt(this.Amount);
  }

  ChangeAmount(n:number){
    this.amount+=n;
    this.Update.emit(this.amount);
  }
}
