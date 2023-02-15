import { trigger, transition, style, animate, state } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { Component,  Input } from '@angular/core';
import { Item } from '../../models/item';
import { Message } from '../../models/message';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
  animations:[
    trigger('SlideIn', [
      transition(":enter", [
        style({transform: "translateX(-100vw)",top:'0vh', position:'absolute'}), 
        animate(
          "200ms ease-in-out",
          style({ transform: "translateX(0vw)" })
        )
      ]),
      transition(":leave", [
        style({  transform: "translateX(0vw)" ,top:'0vh',position:'absolute'}), 
        animate(
          "200ms ease-in-out",
          style({transform: "translateX(100vw)" })
        )
      ]),
    ])
  ]
})

export class SearchComponent {
  items: Item[] = [];
  @Input() Query = '';
  Query2 = ' ';
  page = 0;
  interval : any;
  FadeInfo: boolean = false;
  item:Item|null = null;
  totalPages = 0;

  constructor(private http: HttpClient){
    this.interval = setInterval(() => {
      if(this.Query != localStorage.getItem('query')){
        this.Query = localStorage.getItem('query')+'';
        this.page = 0;
        this.Search();
      }
    }, 100);
  }

  ngOnInit(){
    this.Search();
  }

  Search(){    
    let item2:Item[] = [];
    this.http.get<Message>('http://localhost:4200/api/Item/GetItems?SID='+localStorage.getItem('Session')+'&query='+this.Query+'&page='+this.page)
    .subscribe(data => {
      if(data.code == 'SERVER'){
        clearInterval(this.interval);
      }
      if(data.code == 'WORKED'){
        data.message.items.forEach((el: any) => {
          item2.push(el)
        })
        this.totalPages=data.message.pages
      };
      this.items = item2;
      this.Query2 = this.Query
    });
  }

  OpenInfo(i : Item) {
    this.FadeInfo = true;
    this.item = i;
  }

  Page(n:number) {
    let dots = document.getElementsByClassName("dot");
    for (let i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(" active", "");
    }
    this.page = n;
    this.Search();
    dots[n].className += " active";
  }
  numSequence(n: number): Array<number> {
    return Array(n);
  }
}