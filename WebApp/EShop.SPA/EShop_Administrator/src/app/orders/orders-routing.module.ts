import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {ordersRoutes} from 'src/app/orders/orders-routes';

@NgModule({
  imports: [RouterModule.forChild(ordersRoutes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
