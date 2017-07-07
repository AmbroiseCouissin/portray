import { Component } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    public _age: number = 0;
    public _income: number = 0;
    public _currencySymbol: string;


}
