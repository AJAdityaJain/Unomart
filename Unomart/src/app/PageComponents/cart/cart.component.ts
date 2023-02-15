import { CartItem } from './../../models/cartItem';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message } from '../../models/message';
import { OrderItem } from '../../models/order';
import { Address } from '../../models/user';
import { Currency } from 'src/app/models/currency';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent {
  addresses!:Address[];
  deliveryCharge = 0;

  discount = 0;
  MRP = 0;
  itemsNum = 0;
  Grand = 0;
  
  addressIndex = 0;

  items:CartItem[] = [];
  couponText = [];
  coupon = "";
  symbol = "";

  dc = 0;

  constructor(private router:Router,private route: ActivatedRoute,private http:HttpClient){}
  ngOnInit (){
    let res = new DOMParser().parseFromString( this.GetCurrency().symbol, 'text/html').body.textContent
    this.symbol = res==null?'$':res;
    this.SetItems();
    this.GetAddresses();      
  }
  
  GetCurrency(){
    let c = JSON.parse(localStorage.getItem('currency')+"");
    if(c == 'null')
      c = new Currency();
    return c;
  }

  UpdateTotal(){
    this.http.get<Message>('http://localhost:4200/api/Cart/GetTotal?SID='+localStorage.getItem('Session')).subscribe(data =>{
      if(data.code == 'WORKED'){
        this.MRP = data.message.total;
        this.itemsNum = data.message.items;
        this.Grand = Math.round((((1-this.discount)* this.MRP)+this.deliveryCharge)*100)/100;
      }
    });
  }

  SetItems() {
    this.http.get<Message>('http://localhost:4200/api/Cart/Get?SID='+localStorage.getItem('Session')).subscribe(data =>{
      if(data.code == 'WORKED'){
        this.items = data.message;
        this.SetDeliveryCharge();
      }
    });
  }

  SetDeliveryCharge(){
    this.http.get<Message>('http://localhost:4200/api/Order/GetDeliveryCharge?SID='+localStorage.getItem('Session')).subscribe(data =>{
      if(data.code == 'WORKED'){
        this.deliveryCharge = data.message;
        this.dc = data.message;
        this.UpdateTotal();
      }
    });
  }


  SetAddressIndex($event: Event) {
    let e = $event.target as HTMLSelectElement;
    this.addressIndex = Number.parseInt(e.value);
  }

  checkCoupon(e:Event){
    let el =(e.target as HTMLInputElement);
    this.coupon = el.value;
    this.discount = 0;
    this.deliveryCharge = this.dc;
    this.couponText = [];

    this.http.get<Message>('http://localhost:4200/api/Coupon/Get?SID='+localStorage.getItem('Session')+'&query='+this.coupon)
    .subscribe(data=>{
      el.id = '';
      if(this.coupon != '')
        el.id = 'wrong';
      
      if(data.message.description!=undefined){
        data.message.description = data.message.description.replace('$Currency$', this.symbol);
        this.couponText = data.message.description.split('\n');
      }
      if(data.message.valid == true){
        this.deliveryCharge = data.message.deliveryCharge;
        this.discount = data.message.discount;
        el.id = 'correct';
      }
      else if(data.message.valid == false){
        el.id = 'warn';
      }
      this.SetDeliveryCharge();
      this.UpdateTotal();
    });
  }


  EmptyCart(){
    this.http.delete<Message>('http://localhost:4200/api/Cart/EmptyCart?SID='+ localStorage.getItem('Session'))
    .subscribe(data => {
      if(data.code == 'WORKED'){
        this.http.get<Message>('http://localhost:4200/api/Cart/GetTotal?SID='+localStorage.getItem('Session')).subscribe(data =>{
          if(data.code == "WORKED")
            var str = data.message.items.toString().padStart(2,'0');
            localStorage.setItem("Items", str!='00'?str:'');
        });
        this.SetItems();        
      }
    })    
  }

  Order(){
    let address = this.addresses[this.addressIndex];

    this.http.post<Message>("http://localhost:4200/api/Order/Create?SID="+localStorage.getItem('Session')+"&address="+address.userAddress + "&coupon="+ this.coupon,{}).subscribe(data =>{
      switch(data.code){
        case 'BDREQ1':        
          this.router.navigate(['../../signup'] ,{relativeTo:this.route});
          break;
        case 'SERVER':
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Oops... An error occurred!'}});
          break;
        case 'WORKED':
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Yaay! Order successful'}});
          break;
      } 
      this.EmptyCart();
    });    
  }

  GetAddresses() {
    this.http.get<Message>('http://localhost:4200/api/Address/GetAddress?SID='+localStorage.getItem('Session'))
    .subscribe(data=>{
      this.addresses = [];
      switch(data.code){
        case 'BDREQ1':
          this.router.navigate(['../../signup'] ,{relativeTo:this.route});
          break;
        case 'SERVER':
                    
          setTimeout(() => {
            this.GetAddresses();
          }, 200);
          return;
          // this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Oops... An error occurred!'}});
          break;
        case 'WORKED':
          data.message.forEach((el:Address) => {
            let a = new Address();
            a = el;
            this.addresses.push(a);
          });
        break;
      }
    })
  }

  Search(){
    this.router.navigate(['../search'] , {relativeTo:this.route});
  }
}
