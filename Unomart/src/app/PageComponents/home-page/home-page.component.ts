import { Component } from '@angular/core';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
  
  slideIndex :number= 0;

  constructor(){}
  ngOnInit (){
    this.showSlides(1);
  }

  showSlides(n:number) {
    let i;
    let slides = document.getElementsByClassName("mySlides") as HTMLCollectionOf<HTMLDivElement>;
    let dots = document.getElementsByClassName("dot");

    for (i = 0; i < slides.length; i++) {
      slides[i].style.display = "none";  
    }
    
    this.slideIndex = n;
    
    if (this.slideIndex > slides.length) {this.slideIndex = 1}    
    for (i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[this.slideIndex-1].style.display = "block";  
    dots[this.slideIndex-1].className += " active";
  }
}
