import { Error404Component } from './MiscComponents/error404/error404.component'
import { HomePageComponent } from './PageComponents/home-page/home-page.component';
import { OrderItemsComponent } from './PageComponents/order-items/order-items.component';
import { AboutComponent } from './PageComponents/about/about.component';
import { OrderComponent } from './PageComponents/order/order.component';
import { CartComponent } from './PageComponents/cart/cart.component';
import { ProfileComponent } from './PageComponents/profile/profile.component';
import { SearchComponent } from './PageComponents/search/search.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoaderComponent } from './MiscComponents/loader/loader.component';
import { MessageComponent } from './MiscComponents/message/message.component';
import { SignUpComponent } from './PageComponents/sign-up/sign-up.component';
  

const routes: Routes = [
    { path: '',redirectTo:'signup',pathMatch:'full'},
    { path: 'main', component: HomeComponent,
    children:[
      { path: '',redirectTo:'home',pathMatch:'full'},
      { path: 'home', component: HomePageComponent,data: { animation: 't' }},
      { path: 'search', component: SearchComponent,data: { animation: 't' }},
      { path: 'profile', component: ProfileComponent,data: { animation: 't' }},
      { path: 'cart', component: CartComponent,data: { animation: 't' }},
      { path: 'orders', component: OrderComponent,data: { animation: 't' }},
      { path: 'orderitems', component: OrderItemsComponent,data: { animation: 't' }},
      { path: 'about', component: AboutComponent,data: { animation: 't' }},

      { path: 'message', component: MessageComponent,data: { animation: 'f' }},
    ]
  },
  { path: 'signup', component: SignUpComponent},
  { path: '**', component: Error404Component}
];
  
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }