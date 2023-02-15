import { Router } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-error404',
  templateUrl: './error404.component.html',
  styleUrls: ['./error404.component.scss']
})
export class Error404Component {
  constructor(private router:Router){}
  Home(){
    this.router.navigateByUrl('/signup');
  }
}
