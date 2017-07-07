import { Component, EventEmitter, Input, Output } from '@angular/core';

import { User } from './user';

@Component({
  selector: 'user-submitted',
  template: `
  <div *ngIf="submitted">
    <h2>You submitted the following:</h2>
    <div class="row">
      <div class="col-xs-3">Age</div>
      <div class="col-xs-9 pull-left">{{ user.age }}</div>
    </div>
    <div class="row">
      <div class="col-xs-3">Income</div>
      <div class="col-xs-9 pull-left">{{ user.income }}</div>
    </div>
    <div class="row">
      <div class="col-xs-3">Currency</div>
      <div class="col-xs-9 pull-left">{{ user.currencySymbol }}</div>
    </div>
    <br>
    <button class="btn btn-default" (click)="onClick()">Edit</button>
  </div>`
})
export class SubmittedComponent {
  @Input()  user: User;
  @Input()  submitted = false;
  @Output() submittedChange = new EventEmitter<boolean>();
  onClick() { this.submittedChange.emit(false); }
}