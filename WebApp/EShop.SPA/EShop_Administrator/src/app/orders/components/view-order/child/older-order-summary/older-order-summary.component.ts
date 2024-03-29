import { Component, OnDestroy } from '@angular/core';

import {OrderService} from 'src/app/orders/services/order.service';
import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';

@Component({
  selector: 'app-older-order-summary',
  templateUrl: './older-order-summary.component.html'
})
export class OlderOrderSummaryComponent implements OnDestroy {

  orderSummary: OrderSummaryViewModel[] = [];

  $orderSummary = this._orderService.olderOrderSummary$.subscribe({
    next: data => this.orderSummary = data as OrderSummaryViewModel[],
    error: err => {
      this.httpErrorMessage = err.error.message;
      this.httpStatusIsNotNotFound = err.status !== 404;
    }
  });

  httpStatusIsNotNotFound: boolean = false;

  httpErrorMessage: string | null = null;

  popupText: string = 'Click and see more details for this order';

  constructor(private _orderService: OrderService){ }

  editOlderOrderSummaryCurrentPage(event: number): void{
    this._orderService.currentPageSub.next(event);
  }
  ngOnDestroy(): void {
    this.$orderSummary.unsubscribe();
  }
}
