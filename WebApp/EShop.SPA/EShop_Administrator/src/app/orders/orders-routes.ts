import { Routes } from "@angular/router";
import {ViewOrderComponent} from 'src/app/orders/components/view-order/view-order.component';
import {OrderStatusComponent} from 'src/app/orders/components/order-status/order-status.component';
import {OrderHistoryComponent} from 'src/app/orders/components/order-history/order-history.component';
import {OrderByOrderNumberComponent} from 'src/app/orders/components/order-by-order-number/order-by-order-number.component';

export const ordersRoutes: Routes = [
  {path: '', component: ViewOrderComponent},
  {path: 'detail/:orderNumber',component: OrderByOrderNumberComponent},
  {path: 'status', component: OrderStatusComponent},
  {path: 'history', component: OrderHistoryComponent}
]
