import { trigger, state, style, transition, animate} from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { Component} from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router} from '@angular/router';
import { Message } from '../models/message';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  animations: [
    trigger('Expand', [
      state('true', style({'width':'10rem'})),
      state('false', style({'width':'3.8rem'})),
      transition('true => false', animate('150ms linear')),
      transition('false => true', animate('150ms linear'))
    ])
  ]
})
export class HomeComponent {
  Expand = false;
  Query = '';
  Shimmer = false;
  Colors = [
    "#00916E",
    "#4DAA57",
    "#C19AB7",
    "#70AE6E",
    "#1E555C",
    "#DE4D86",
    "#3F2A2B",
    "#FFF07C",
    "#FF0080",
    "#E26D5C",
    "#E26D5C",
    "#84E6F8"
  ];


  constructor(private http:HttpClient,private route: ActivatedRoute,  private router:Router){}
  ngOnInit(){
    this.router.events.subscribe((val) => {
        if(val instanceof NavigationStart){
          this.Shimmer = true;
        }
        if(val instanceof NavigationEnd){
            setTimeout(() => {
              this.Shimmer = false;
            }, 400);
        }
    });
    this.Theme()
    this.http.get<Message>('http://localhost:4200/api/User/Get?SID='+localStorage.getItem('Session'))
    .subscribe(data => {
      switch(data.code){
        case 'BDREQ1':
          this.router.navigate(['../signup'] ,{relativeTo:this.route});
          break;
        case 'SERVER':
          this.router.navigate(['../signup'] ,{relativeTo:this.route});
          break;
        case 'WORKED':
          if(data.message.username == null){
            this.router.navigate(['../signup'] ,{relativeTo:this.route});
          }
          let a = document.getElementById('icon') as HTMLDivElement;            
          a.innerHTML = data.message.username.substring(0,2);
          let n = 0;
          n = data.message.username.charCodeAt(0) + data.message.username.charCodeAt(1);
          a.style.setProperty('background-color',this.Colors[n%this.Colors.length]);  
        break;
      }      
    })
  }
 
  Status(){
    return localStorage.getItem('Items')
  }

  Home(){
    this.routeTo('home');
  }
  Search(ev : any, b : boolean){
    if(b||ev.key == 'Enter'){
      localStorage.setItem('query', this.Query);    
      this.routeTo('search');
    }
  }
  Profile(){
    this.routeTo('profile');
    // this.router.navigate(['profile'] , {relativeTo:this.route});      
  }

  Theme() {
    let a = document.getElementsByClassName('Theme')[0] as HTMLSpanElement;    
    let b = document.getElementById('Theme') as HTMLSpanElement;
    switch(a.id){
      case 'dark':
        a.id = 'light'
        b.innerText = 'Dark Theme'
        break;
      case 'light':
        a.id = 'dark'
        b.innerText =  'Light Theme'
        break;
      default:
          let c = window.matchMedia('(prefers-color-scheme: light)').matches;
          if(c){
            a.id = 'light'
            b.innerText = 'Dark Theme'
          }
          else{
            a.id = 'dark'
            b.innerText = 'Light Theme'
          }
          break;
  
    }
    // const active = this.themeService.getActiveTheme() ;
    // if (active.name === 'light') {
      // this.themeService.setTheme('dark');
    // } else {
      // this.themeService.setTheme('light');
    // }
  }

  
  Logout(){
    localStorage.removeItem('Session');
    this.router.navigate(['../signup'] ,{relativeTo:this.route});
  }

  Cart(){
    this.routeTo('cart');
  }
  Orders(){
    this.routeTo('orders');
  }
  About(){
    this.routeTo('about');
  }  
  routeTo(ref:string) {
    let string = this.router.url.slice(12);
    string = string.substring(0,string.length-1)
    if(string == ref){return}
    // return this.contexts.getContext('primary')?.route?.snapshot?.data?.['animation'];
     this.router.navigate([ref] , {relativeTo:this.route});      
  }

}
