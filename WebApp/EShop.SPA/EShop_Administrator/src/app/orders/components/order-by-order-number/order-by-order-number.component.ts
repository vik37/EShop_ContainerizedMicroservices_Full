import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription, EMPTY } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { faEarth, faGlobe, faCity, faRoad, faHouseUser, faLocation }
        from '@fortawesome/free-solid-svg-icons';
import {OrderService} from 'src/app/orders/services/order.service';
import {Order} from 'src/app/orders/models/order';


@Component({
  selector: 'app-order-by-order-number',
  templateUrl: './order-by-order-number.component.html'
})
export class OrderByOrderNumberComponent implements OnInit, OnDestroy {

  private _orderSubscrition = new Subscription();
  private _orderStatusSubscrition = new Subscription();
  public order: Order | undefined;
  icons: any = {faHouseUser:faHouseUser,faEarth:faEarth,faGlobe:faGlobe,
    faCity:faCity,faRoad:faRoad,faLocation:faLocation}
  orderStatusProgress: number = 0;

  private _currentStatus: string = "";

  constructor(private _router: Router, private _activatedRouter: ActivatedRoute,
                private _orderService: OrderService){ }

  ngOnInit(): void {
    this.loadData();
    this.getOrderStatusProgress();
  }

  loadData(): void{
    this._orderSubscrition = this._activatedRouter.paramMap.pipe(
      switchMap(params => {
        let orderNumber = params.get("orderNumber");
        if (orderNumber && orderNumber.match(/^[0-9]+$/)) {
          return this._orderService.getOrderByOrderNumber(orderNumber);
        } else {
          return EMPTY;
        }
      })
    ).subscribe({
      next: data => {
        this._currentStatus = data.status;
        if (data) {
          this.order = data;
        }
      },
      error: () => this.onBack()
    });

  }

  getOrderStatusProgress(): void{
    this._orderStatusSubscrition = this._orderService.orderStatus$
                    .pipe(
                      map(x => x.filter(y=> y.id !== 6))
                    )
                    .subscribe({
                                next: data => {
                                  setTimeout(()=>{
                                    let currentStatus = data.find(x => x.name === this._currentStatus);
                                    if(currentStatus){
                                      this.orderStatusProgress = currentStatus.id/data.length*100;
                                    }
                                  },1000);
                                }
                    });
  }

  onBack(): void{
    this._router.navigate(['/orders']);
  }

  ngOnDestroy(): void {
    this._orderSubscrition.unsubscribe();
    this._orderStatusSubscrition.unsubscribe();
  }
}
