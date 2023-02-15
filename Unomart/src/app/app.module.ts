import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app.routing-module';
import { AppComponent } from './app/app.component';
import { HomeComponent } from './home/home.component';
import { LoaderComponent } from './MiscComponents/loader/loader.component';
import { MessageComponent } from './MiscComponents/message/message.component';
import { ShimmerComponent } from './MiscComponents/shimmer/shimmer.component';
import { AboutComponent } from './PageComponents/about/about.component';
import { CartComponent } from './PageComponents/cart/cart.component';
import { HomePageComponent } from './PageComponents/home-page/home-page.component';
import { OrderItemsComponent } from './PageComponents/order-items/order-items.component';
import { OrderComponent } from './PageComponents/order/order.component';
import { SignUpComponent } from './PageComponents/sign-up/sign-up.component';
import { ProfileComponent } from './PageComponents/profile/profile.component';
import { SearchComponent } from './PageComponents/search/search.component';
import { CategorySearchComponent } from './SubComponents/category-search/category-search.component';
import { ItemTileComponent } from './SubComponents/item-tile/item-tile.component';
import { OrderTileComponent } from './SubComponents/order-tile/order-tile.component';
import { SearchResultInfoComponent } from './SubComponents/search-result-info/search-result-info.component';
import { NumInputComponent } from './SubComponents/num-input/num-input.component';
import { Error404Component } from './MiscComponents/error404/error404.component';

@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    OrderTileComponent,
    ProfileComponent,
    HomeComponent,
    SearchComponent,
    LoaderComponent,
    CartComponent,
    OrderComponent,
    MessageComponent,
    AboutComponent,
    OrderItemsComponent,
    SearchResultInfoComponent,
    HomePageComponent,
    CategorySearchComponent,
    ShimmerComponent,
    ItemTileComponent,
    NumInputComponent,
    Error404Component,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
