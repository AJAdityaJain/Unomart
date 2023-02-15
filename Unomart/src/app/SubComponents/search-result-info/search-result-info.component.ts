import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search-result-info',
  templateUrl: './search-result-info.component.html',
  styleUrls: ['./search-result-info.component.scss']
})
export class SearchResultInfoComponent {
  @Input() name = '';
  @Input() src = '';
  @Input() cost = '';
  @Input() description = '';
  @Input() quantity = '';
  @Output() back:EventEmitter<string> = new EventEmitter<string>();

  constructor(){}

  ngOnInit (){
    this.description = this.description.replaceAll('\\p','&#x2022;\\t');      
    this.description = this.description.replaceAll('\\n','<br>');      
    this.description = this.description.replaceAll('\\t','&emsp;');      
    let a = document.getElementById('Desc') as HTMLDivElement;
    a.innerHTML = this.description;    
  }

  Search(){
    this.back.emit('true');
  }
}
