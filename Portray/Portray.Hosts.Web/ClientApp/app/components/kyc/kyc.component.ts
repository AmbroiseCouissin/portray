import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { User } from '../../shared/user';

@Component({
    selector: 'kyc',
    templateUrl: './kyc.component.html'
})
export class KycComponent {
    currencySymbols = ['HKD', 'USD', 'CNY', 'EUR'];
    submitted = false;
    user: User = new User(18, 400000, this.currencySymbols[0]);

    onSubmit() {
        this.submitted = true;
    }

    active = true;

    addUser() {
        this.user = new User(18, 400000, '');
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }
}
