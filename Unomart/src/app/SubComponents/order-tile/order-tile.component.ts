import { Component,  Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Currency } from 'src/app/models/currency';

@Component({
  selector: 'app-order-tile',
  templateUrl: './order-tile.component.html',
  styleUrls: ['./order-tile.component.scss']
})
export class OrderTileComponent {
  @Input() Total:string = '';
  @Input() DateTime:string = '';
  @Input() OID:string = '';
  @Input() DeliveryCharge:string = '';
  @Input() Discount:string = '';
  @Input() Items:string = '';
  @Input() Address:string = '';
 
  symbol:string = '$';
  GrandTotal = 0;
  TimeText:string = '';


  constructor(private route: ActivatedRoute,  private router:Router){}
  ngOnInit (){
    this.GrandTotal = (Number.parseFloat(this.Total)*(100-Number.parseFloat(this.Discount))/100.0)+Number.parseFloat(this.DeliveryCharge);
    this.GrandTotal = Math.round(100*this.GrandTotal)/100;
    this.TimeText = this.GetTime();          
    this.Discount = (Math.round(Number.parseFloat(this.Discount)*100)/100).toString();
    let res = new DOMParser().parseFromString( this.GetCurrency().symbol, 'text/html').body.textContent
    this.symbol = res==null?'$':res;
  }  
  GetCurrency(){
    let c = JSON.parse(localStorage.getItem('currency')+"");
    if(c == 'null')
      c = new Currency();
    return c;
  }


  GetTime(){
    return this.convertMsToTime( new Date().getTime() - Date.parse(this.DateTime));
  }

  TabParent():void{
    this.router.navigate(['../orderitems'] , {relativeTo:this.route,queryParams:{OID:this.OID}});     
  }

  convertMsToTime(milliseconds: number) {
    // console.trace()
    let seconds = Math.floor(milliseconds / 1000);
    let minutes = Math.floor(seconds / 60);
    let hours = Math.floor(minutes / 60);
    let days = Math.floor(hours / 24);
    seconds = seconds % 60;
    minutes = minutes % 60;
  
    if(minutes <2)
      return seconds + ' seconds';
    
  
    if(hours < 2)          
      return minutes + ' minutes';
    
    if(days < 2)     
      return hours + ' hours'
    
    return days + ' days'
  }
}

