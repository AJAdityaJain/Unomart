import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, enableProdMode } from "@angular/core";
import { Message } from '../models/message';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private router:Router,private a:ActivatedRoute, private http :HttpClient){}
  ngOnInit(){
    enableProdMode();
    console.clear()
  }
}