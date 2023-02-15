import { HttpClient } from '@angular/common/http';
import { Currency } from './../../models/currency';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Message } from 'src/app/models/message';

@Component({
  selector: 'app-item-tile',
  templateUrl: './item-tile.component.html',
  styleUrls: ['./item-tile.component.scss']
})
export class ItemTileComponent {
  @Input() itemName = '';
  @Input() itemImage = '';
  @Input() itemPrice = '';
  @Input() description = '';
  @Input() itemQuantity = '';

  @Input() disable = '';
  @Input() showTotal:any = undefined;
  @Input() amount = '';


  Amount:number = 0;
  Price:number = 0;
  Grand:number = 0;
  symbol:string = '$';
  
  b = false;
  limit = true;

  @Output() imgclick : EventEmitter<string>  = new EventEmitter<string>() 

  constructor(private http:HttpClient){   }
  ngOnInit (){
    this.Price = Number.parseFloat(this.itemPrice);
    if(this.disable != 'true'){
      this.http.get<Message>('http://localhost:4200/api/Cart/GetItem?SID='+localStorage.getItem('Session')+'&Item='+this.itemName)
      .subscribe(data => {
        if(data.code == 'WORKED'){
          this.Amount = data.message;
          this.b = true;
          this.Grand = Math.round( 100* this.Amount* this.Price)/100;
        }
      })
    }
    else{
      this.Amount = Number.parseInt(this.amount);
      this.b = true;
      this.Grand = Math.round( 100* this.Amount* this.Price)/100;
    }
    let res = new DOMParser().parseFromString( this.GetCurrency().symbol, 'text/html').body.textContent
    this.symbol = res==null?'$':res;
  }

  GetCurrency(){
    let c = JSON.parse(localStorage.getItem('currency')+"");
    if(c == 'null')
      c = new Currency();
    return c;
  }

  Emit(){
    this.imgclick.emit('true')
  }

  SetAmount(n:number){
    if(this.limit){
      this.limit = false;
      this.http.put<Message>('http://localhost:4200/api/Cart/SetItem?SID='+localStorage.getItem('Session')+'&Item='+this.itemName+'&Value='+n,{})
      .subscribe(data => {
        if(data.code == 'WORKED'){
          this.limit = true;
          this.Amount = n;
          this.Grand = Math.round( 100* this.Amount* this.Price)/100;
          this.http.get<Message>('http://localhost:4200/api/Cart/GetTotal?SID='+localStorage.getItem('Session')).subscribe(data =>{
            if(data.code == "WORKED")
              var str = data.message.items.toString().padStart(2,'0');
              localStorage.setItem("Items", str!='00'?str:'');
          })
        }
      })
    }
  }
}
