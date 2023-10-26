import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import {OrderService} from 'src/app/orders/services/order.service';
import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';

@Component({
  selector: 'app-latest-order-summary',
  templateUrl: './latest-order-summary.component.html'
})
export class LatestOrderSummaryComponent implements OnDestroy{
  orderSummary: OrderSummaryViewModel[] = [];
  orderSubscription: Subscription = new Subscription();
  httpStatusIsNotNotFound: boolean = false;

  httpErrorMessage: string | null = null;

  popupText: string = 'Click and see more details for this order';

  constructor(private _orderService: OrderService){ }

  latestOrder$ = this._orderService.latestOrder$
    .subscribe({
      next: data => this.orderSummary = data as OrderSummaryViewModel[],
      error: err => {
        this.httpErrorMessage = err.error.message;
        this.httpStatusIsNotNotFound = err.status !== 404;
      }
    });

  ngOnDestroy(): void {
    this.latestOrder$.unsubscribe();
  }
}
