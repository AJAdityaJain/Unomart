import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { Item } from '../../models/item';
import { Message } from '../../models/message';

@Component({
  selector: 'app-category-search',
  templateUrl: './category-search.component.html',
  styleUrls: ['./category-search.component.scss']
})
export class CategorySearchComponent {
  @Input() category = '';

  items:Item[] | undefined;

  constructor(private http:HttpClient, private router : Router, private route : ActivatedRoute){}
  ngOnInit (){
    this.category =this.category.charAt(0).toUpperCase() + this.category.slice(1);
    this.SearchFor();
  }

  SearchFor(){
    let item2:Item[] = [];
    this.http.get<Message>('http://localhost:4200/api/Item/GetItems?SID='+localStorage.getItem('Session')+'&query='+this.category)
    .subscribe(data => {
    if(data.code == 'SERVER'){
      setTimeout(() => {
        this.SearchFor()
      }, 500);
    }
    else if(data.code == "WORKED"){
      data.message.items.forEach((el: any) => {
        let I = new Item();
        I = el;
        item2.push(I)
      })
    };
    this.items = item2;
  });

  }

  GetAmount(name: string|undefined) {
    // if(name == undefined)
    //   return;

    // let a = cart.getItem(name)[1];
    // if(a != null)
    // return a.amount;

    return 0;
  }
  
  Search(){
    localStorage.setItem('query', this.category);
    this.router.navigate(['../search'] ,{relativeTo:this.route});
  }
}