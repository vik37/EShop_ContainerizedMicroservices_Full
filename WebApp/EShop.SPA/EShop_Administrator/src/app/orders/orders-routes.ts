import { Routes } from "@angular/router";
import {ViewOrderComponent} from 'src/app/orders/components/view-order/view-order.component';
import {OrderStatusComponent} from 'src/app/orders/components/order-status/order-status.component';
import {OrderHistoryComponent} from 'src/app/orders/components/order-history/order-history.component';

export const ordersRoutes: Routes = [
  {path: '', component: ViewOrderComponent},
  {path: 'status', component: OrderStatusComponent},
  {path: 'history', component: OrderHistoryComponent}
]
