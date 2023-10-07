import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import {ViewOrderComponent} from 'src/app/orders/components/view-order/view-order.component';
import {OrderStatusComponent} from 'src/app/orders/components/order-status/order-status.component';
import {OrderHistoryComponent} from 'src/app/orders/components/order-history/order-history.component';
import { OrdersRoutingModule } from 'src/app/orders/orders-routing.module';

import {OrderService} from 'src/app/orders/services/order.service';

@NgModule({
  declarations: [
    ViewOrderComponent,
    OrderStatusComponent,
    OrderHistoryComponent
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    HttpClientModule
  ],
  providers:[OrderService]
})
export class OrdersModule { }
