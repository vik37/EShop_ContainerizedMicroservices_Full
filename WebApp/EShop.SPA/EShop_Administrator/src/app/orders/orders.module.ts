import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import {ViewOrderComponent} from 'src/app/orders/components/view-order/view-order.component';
import {OrderStatusComponent} from 'src/app/orders/components/order-status/order-status.component';
import {OrderHistoryComponent} from 'src/app/orders/components/order-history/order-history.component';
import {OrdersRoutingModule} from 'src/app/orders/orders-routing.module';
import {SharedModule} from 'src/app/shared/shared.module';
import {PagginationService} from 'src/app/shared/services/paggination.service';

import {OrderService} from 'src/app/orders/services/order.service';
import {OrderByOrderNumberComponent} from 'src/app/orders/components/order-by-order-number/order-by-order-number.component';
import { LatestOrderSummaryComponent } from './components/view-order/child/latest-order-summary/latest-order-summary.component';
import { OlderOrderSummaryComponent } from './components/view-order/child/older-order-summary/older-order-summary.component';
import { OrderItemComponent } from './components/shared/order-item/order-item.component';

@NgModule({
  declarations: [
    ViewOrderComponent,
    OrderStatusComponent,
    OrderHistoryComponent,
    OrderByOrderNumberComponent,
    LatestOrderSummaryComponent,
    OlderOrderSummaryComponent,
    OrderItemComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    OrdersRoutingModule,
    HttpClientModule,
    SharedModule
  ],
  providers:[OrderService]
})
export class OrdersModule { }
