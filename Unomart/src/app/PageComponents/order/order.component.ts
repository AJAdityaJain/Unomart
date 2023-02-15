import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message } from '../../models/message';
import { Order } from '../../models/order';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent {

  orders:Order[] = [];


  constructor(private router:Router,private route: ActivatedRoute,private http:HttpClient){
    this.GetOrder()
  }

  GetOrder(){
    this.http.get<Message>('http://localhost:4200/api/Order/Get?SID='+localStorage.getItem('Session'))
    .subscribe(data=>{
      if(data.code == 'WORKED'){
        this.orders = [];
        data.message.forEach((el:Order) => {
          let o = new Order()
          o = el;
          this.orders.push(o);
        });
        if(this.orders.length == 0){
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'You haven\'t made any orders! This needs to be undone'}});
        }
      }
      else{
        if(data.code == 'SERVER'){
          this.router.navigate(['../message'] ,{relativeTo:this.route, queryParams:{m:'Oops... An error occurred!'}});
          return;
        }
        if(data.code == 'BDREQ1'){
          this.router.navigate(['../../signup'] ,{relativeTo:this.route});
          return;
        }  
      }
    })
  }

}
