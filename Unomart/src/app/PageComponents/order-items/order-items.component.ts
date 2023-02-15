import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message } from '../../models/message';
import { OrderItem } from '../../models/order';

@Component({
  selector: 'app-order-items',
  templateUrl: './order-items.component.html',
  styleUrls: ['./order-items.component.scss']
})
export class OrderItemsComponent {
  orderItems!:OrderItem[];
  OID:string|null = '';

  constructor(private http:HttpClient,private route: ActivatedRoute,  private router:Router){
    this.OID = new URLSearchParams(window.location.search).get('OID');
    this.GetOrderItems()
  }  

  GetOrderItems(){
    this.orderItems = []
    this.http.get<Message>('http://localhost:4200/api/Item/GetOrderItems?OID='+this.OID + '&SID='+localStorage.getItem('Session'))
    .subscribe(data=>{
      switch(data.code){
        case 'SERVER':
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Oops... An error occurred!'}});
          return;
        break;
        case 'BDREQ1':
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Oops... An error occurred!'}});
          return;
        break;
        case 'WORKED':
          data.message.forEach((el: any) => {
            let oi = new OrderItem();
            oi.ItemAmount = el.itemAmount;
            oi.ItemName = el.itemName;
            oi.ItemCost = el.itemCost/el.itemAmount;
            oi.ItemImg = el.itemImage;
            
            this.orderItems.push(oi);
          });
        break;
      }
    })
  }

  Orders(){
    this.router.navigate(['../orders'] , {relativeTo:this.route});     
  }
}
