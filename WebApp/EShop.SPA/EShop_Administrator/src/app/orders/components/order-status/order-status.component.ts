import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription, EMPTY } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { OrdersByOrderStatus } from 'src/app/orders/models/orders-by-order-status';
import { OrderStatus } from 'src/app/orders/models/order-status';
import { OrderService } from 'src/app/orders/services/order.service';

@Component({
  selector: 'app-order-status',
  templateUrl: './order-status.component.html'
})
export class OrderStatusComponent implements OnInit, OnDestroy{

  private _orderSubscrition = new Subscription();
  private _orderStatusSubscrition = new Subscription();

  ordersByOrderStatus: OrdersByOrderStatus[] = [];
  orderStatus: OrderStatus[] = [];
  statusName:string = '';
  buttonText: string = 'status';

  constructor(private _orderService: OrderService,
                private _activatedRoute: ActivatedRoute, private _router: Router){}

  ngOnInit(): void {
    this.loadData();
    this.getOrderStatus();
  }

  loadData(): void{
    this._orderSubscrition = this._activatedRoute.paramMap.pipe(
      switchMap(params => {
        let statusId = params.get("statusId");
        if(statusId && statusId.match(/^[0-9]+$/)){
          return this._orderService.getOrdersByOrderStatus(statusId);
        }
        else{
          return EMPTY;
        }
      })
    )
    .subscribe({
      next: data => {
        console.log("orders: ", data);
        this.ordersByOrderStatus = data;
        this.statusName = data[0].statusName;
      },
      error: () => this.onBack()
    })
  }

  getOrderStatus(): void{
    this._orderStatusSubscrition = this._orderService.orderStatus$.subscribe({
      next: data => {
        this.orderStatus = data;
      }
    })
  }

  onBack(): void{
    this._router.navigate(['/orders']);
  }

  ngOnDestroy(): void {
    this._orderSubscrition.unsubscribe();
    this._orderStatusSubscrition.unsubscribe();
  }
}
