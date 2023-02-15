import { Currency } from './../../models/currency';
import { Address, User } from '../../models/user';
import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { Message } from '../../models/message';
import { animate, keyframes, state, style, transition, trigger } from '@angular/animations';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  animations: [
    trigger('Shake', [
      state('true', style({'box-shadow':'0 0 10px red','border-color':'red'})),
      state('false', style({})),
      transition('true => false', animate('200ms ease-in')),
      transition('false => true', animate('300ms', keyframes([
        style({'margin-left':'0px','border-color':'red','box-shadow':'0 0 10px red'}),
        style({'margin-left':'5px'}),
        style({'margin-left':'-5px'}),
        style({'margin-left':'0px'})
      ])))
    ])
  ]
})
export class ProfileComponent {
  @Input() UID:string = '';
  Email:string = '';
  Username:string = '';
  OldPw:string = '';
  NewPw:string = '';
  address: Address[] = [];
  Message: string = '';
  ArgError: boolean = false;
  Strength: number = 0;
  Currencies: Currency[] = [];
  code = 'USD';

  constructor(private router:Router,private route: ActivatedRoute,private http:HttpClient){}

  ngOnInit(){
    this.http.get<Message>('http://localhost:4200/api/User/Get?SID='+localStorage.getItem('Session')).subscribe(data => {
      switch(data.code){
        case 'BDREQ1':
          this.router.navigate(['../../signup'] ,{relativeTo:this.route});
          break;
        case 'SERVER':
          this.router.navigate(['../../signup'] ,{relativeTo:this.route});
          break;
        case 'WORKED':
          this.Username = data.message.username;
          this.Email = data.message.email;
          this.read();
        break;
      }      
    });

    this.http.get<Message>('http://localhost:4200/api/Region/Get').subscribe(data =>{
      if(data.code == 'WORKED'){
        this.Currencies = data.message;
        this.Currencies.forEach(el =>{
          let res = new DOMParser().parseFromString( el.symbol, 'text/html').body.textContent
          el.symbol = res==null?'$':res;      
        })
        setTimeout(() => {
          this.setDefaultValue();
        }, 200);
      }
    })
  }

  setDefaultValue(){
    this.http.get<Message>('http://localhost:4200/api/Region/GetCurrency?SID='+localStorage.getItem('Session')).subscribe(data =>{
      if(data.code == 'WORKED'){
        this.code = data.message.code;
        let a = (document.getElementById(this.code) as HTMLOptionElement);
        a.setAttribute('selected','selected');
      }
    })        
  }

  setCurrency($event: Event) {
    let e = $event.target as HTMLSelectElement;
    
    console.log(e.value,e.selectedOptions[0].innerHTML);

    this.http.put<Message>('http://localhost:4200/api/Region/Update?SID='+ localStorage.getItem('Session') + "&CurrencyCode=" + e.value,{})
    .subscribe(data => {
      if(data.code == 'WORKED'){        
        this.http.get<Message>('http://localhost:4200/api/Region/GetCurrency?SID='+localStorage.getItem('Session')).subscribe(data => {
          if(data.code == 'WORKED'){
            localStorage.setItem('currency', JSON.stringify(data.message))
          }
        });      
      }
    })
  }

  add() {
    this.http.post<Message>('http://localhost:4200/api/Address/CreateAddress?SID='+localStorage.getItem('Session') + "&Address=",{})
    .subscribe(data => {
      if(data.code == 'WORKED'){
        this.read();
      }
      else{
        console.log(data.message)
      }
    })    
  }
  
  update(event:Event) {
    let e = event.target as HTMLInputElement;
    this.http.put<Message>('http://localhost:4200/api/Address/UpdateAddress?SID='+localStorage.getItem('Session')+'&AID='+ e.id + "&address=" + e.value,{})
    .subscribe(data => {
      if(data.code == 'WORKED'){
        this.read();
      }
    })    
  }  

  remove(event:MouseEvent) {
    let e = (event.target as HTMLDivElement);
    this.http.delete<Message>('http://localhost:4200/api/Address/DeleteAddress?AID='+ e.id+"&SID=" + localStorage.getItem('Session'))
    .subscribe(data => {
      if(data.code == 'WORKED'){
        this.read();
      }
    })    
  }  

  read(){
    this.http.get<Message>('http://localhost:4200/api/Address/GetAddress?SID='+localStorage.getItem('Session')).subscribe(data =>{
      this.address = [];
      if(data.code = 'WORKED'){
        data.message.forEach((add:Address) => {
            let a = new Address();
            add.uid = '';
            a = add;
            this.address.push(a);
        });
      }
    });
  }

  Update(){
    let u = new User();
    
    u.email = this.Email;
    u.hash = this.OldPw;
    u.userName = this.Username;


    this.http.put<Message>('http://localhost:4200/api/User/Update?newpw='+this.NewPw, u).subscribe(data =>{
      switch(data.code){
        case 'BDREQ4':
          this.ArgError = true;
          this.Message = "All fields must be filled properly";                  
        break;
        case 'BDREQ1':
          this.ArgError = true;
          this.Message = "Username must contain 4 characters";                  
        break;
        case 'BDREQ2':
          this.ArgError = true;
          this.Message = "New Password must contain 6 characters";                  
        break;
        case 'BDREQ3':
          this.ArgError = true;
          this.Message = "Incorrect Password - Couldn't Update";                  
        break;
        case 'SERVER':
          this.ArgError = true;
          this.Message = "SERVER ERROR";                  
        break;
        case 'WORKED':        
          this.router.navigate(['../search'] ,{relativeTo:this.route});
        break;
      }
    })    
  }
  Check(){
    this.Strength = 1;

    if(this.NewPw.length > 8)
      this.Strength++;
    if(this.NewPw.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/))
      this.Strength++;
    if(this.NewPw.match(/([!,%,&,@,#,$,^,*,?,_,~])/))
      this.Strength++;
    if (this.NewPw.match(/([0-9])/))
      this.Strength++;

    let color = 'maroon';
    switch(this.Strength){
      case 1:
        color = 'maroon'
        break;
        case 2:
          color = 'red'
          break;      case 3:
        color = 'orange'
        break;      case 4:
        color = 'yellow'
        break;      case 5:
        color = 'limegreen'
        break;
    }
    
    document.getElementById('meter')?.style.setProperty('width',(60*((this.Strength-1)/4) +5).toString() + '%');
    document.getElementById('meter')?.style.setProperty('background-color',color);

  }

}