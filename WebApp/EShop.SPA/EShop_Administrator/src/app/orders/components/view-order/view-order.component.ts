import { Component, OnInit, OnDestroy } from '@angular/core';
import {OrderService} from 'src/app/orders/services/order.service';
import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-view-order',
  templateUrl: './view-order.component.html'
})
export class ViewOrderComponent implements OnInit, OnDestroy{

  orderSummary: OrderSummaryViewModel[] = [];
  orderSubscription: Subscription = new Subscription();
  constructor(private orderService: OrderService){}
  ngOnInit(): void {
    this.orderSubscription.add(this.orderService.getAllOrderSummary().subscribe(data =>{
      console.log('Order Summary', data);
      this.orderSummary = data;
    }))
  }

  ngOnDestroy(): void {
    this.orderSubscription.unsubscribe();
  }
}
