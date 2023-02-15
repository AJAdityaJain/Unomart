import { HttpClient} from '@angular/common/http';
import { animate, keyframes, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { User } from '../../models/user';
import { Message } from '../../models/message';
import { Router} from '@angular/router';


@Component({

  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
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
    ]),
    trigger('SlideRight', [
      state('true', style({'transform':'translateX(0%)'})),
      state('false', style({'transform':'translateX(-100%)'})),
      transition('false => true', animate('300ms', keyframes([
        style({'transform':'translateX(-100%)'}),
        style({'transform':'translateX(0%)'})
      ]))),
      transition('true => false', animate('300ms', keyframes([
        style({'transform':'translateX(-100%)'})
      ])))
    ]),
    trigger('SlideLeft', [
      state('true', style({'transform':'translateX(0%)'})),
      state('false', style({'transform':'translateX(100%)'})),
      transition('false => true', animate('300ms', keyframes([
        style({'transform':'translateX(100%)'}),
        style({'transform':'translateX(0%)'})
      ]))),
      transition('true => false', animate('300ms', keyframes([
        style({'transform':'translateX(100%)'})
      ])))
    ])
  ]
})
export class SignUpComponent {
  Email = "";
  UserName = "";
  UserPW  = "";
  Address  = "";
  ArgError = false;
  SlideState = true;
  ErrorMessage = "";

  Strength = 4;

  Show = false;

  constructor(private http:HttpClient, private router:Router){}
  ngOnInit(){
    setTimeout(() => {
      this.http.get<Message>('http://localhost:4200/api/User/AutoLogin?SID='+localStorage.getItem('Session'))
      .subscribe(data => {
          if(data.code == "WORKED" && data.message == true){
            this.http.get<Message>('http://localhost:4200/api/Region/GetCurrency?SID='+localStorage.getItem('Session')).subscribe(data => {
              if(data.code == 'WORKED'){
                localStorage.setItem('currency', JSON.stringify(data.message))
                this.router.navigateByUrl('/main')
              }
            });
          } 
          this.Show = true;
          setTimeout(() => {
            this.Check();
          }, 100);
      });
      
        
    }, 1000);
  }
  
  SignIn(){
    if(this.Email == ""|| this.UserPW == ""){
      this.ArgError = true;
      this.ErrorMessage = "All fields must be filled properly"
      return
    }
    this.http.get<Message>('http://localhost:4200/api/User/Login?email='+this.Email+"&pw="+this.UserPW)
    .subscribe(data => {
      switch(data.code){
        case 'BDREQ1':
          this.ArgError = true;
          this.ErrorMessage = "'" + this.Email + "' is not a Valid E-Mail ID"
        break;
        case 'BDREQ4':
          this.ArgError = true;
          this.ErrorMessage = "All fields must be filled properly"
        break;
        case 'ABSENT':
          this.ArgError = true;
          this.ErrorMessage = "'"+this.Email+"' doesn't have an account. Try signing up";
        break;
        case 'SERVER':
          this.ArgError = true;
          this.ErrorMessage = "An unexpected server-side error occured";
        break;
        case 'WORKED':
          if(data.message == '0'){
            this.ArgError = true;
            this.ErrorMessage = "Wrong Password"
  
          }
          else{
            localStorage.setItem('Session', data.message);          
            this.http.get<Message>('http://localhost:4200/api/Region/GetCurrency?SID='+localStorage.getItem('Session')).subscribe(data2 => {
              if(data2.code == 'WORKED'){
                localStorage.setItem('currency', JSON.stringify(data2.message));
                this.router.navigateByUrl('/main')
              }
            });  
          }
          break;
      } 
    });
  }

  SignUp(){
    let user = new User();
    user.email = this.Email;
    user.hash = this.UserPW;
    user.userName = this.UserName;
    if(this.Address == ''){
      this.ArgError = true;
      this.ErrorMessage = "All fields must be filled properly"
      return;
    }

    this.http.post<Message>('http://localhost:4200/api/User/Create?address='+this.Address,user)
    .subscribe(data => {
      switch(data.code){
        case 'PRESNT':
          this.ArgError = true;
          this.ErrorMessage = "'"+this.Email+"' already has an account. Try signing in";
        break;
        case 'BDREQ1':
          this.ArgError = true;
          this.ErrorMessage = "'" + this.Email + "' is not a Valid E-Mail ID"
        break;
        case 'BDREQ2':
          this.ArgError = true;
          this.ErrorMessage = "Password must contain 6 characters"
        break;
        case 'BDREQ3':
          this.ArgError = true;
          this.ErrorMessage = "Username must contain 4 characters"
        break;
        case 'BDREQ4':
          this.ArgError = true;
          this.ErrorMessage = "All fields must be filled properly"
        break;
        case 'WORKED':
          localStorage.setItem('Session', data.message)          
          this.router.navigateByUrl('/signup')
          break;
      } 
    });
  }

  Slide(){
    this.SlideState = !this.SlideState;
    this.ArgError = false
  }

  Check(){
    this.Strength = 1;

    if(this.UserPW.length > 8)
      this.Strength++;
    if(this.UserPW.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/))
      this.Strength++;
    if(this.UserPW.match(/([!,%,&,@,#,$,^,*,?,_,~])/))
      this.Strength++;
    if (this.UserPW.match(/([0-9])/))
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
    
    document.getElementById('meter')?.style.setProperty('width',(80*((this.Strength-1)/4) +5).toString() + '%');
    document.getElementById('meter')?.style.setProperty('background-color',color);

  }
}