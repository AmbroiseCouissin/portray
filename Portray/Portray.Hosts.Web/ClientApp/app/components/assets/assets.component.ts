import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'assets',
    templateUrl: './assets.component.html',
    styleUrls: ['./assets.component.css'],
})
export class AssetsComponent {
    dataSource: Asset[];
    types: Type[] = [
        { "ID": 1, "Name": "Stock" },
        { "ID": 2, "Name": "Bond" },
        { "ID": 3, "Name": "ETF" },
        { "ID": 4, "Name": "Commodity" },
        { "ID": 5, "Name": "Real Estate" },
        { "ID": 6, "Name": "Mutual Fund" },
        { "ID": 7, "Name": "Currency" },
    ];
    events: Array<string> = [];


    logEvent(eventName) {
        this.events.unshift(eventName);
    }

    clearEvents() {
        this.events = [];
    }
}

export class Type {
    ID: number;
    Name: string;
}

export class Asset {
    type: string;
    symbol: string;
    unitValue: number;
    quantity: number;
}


