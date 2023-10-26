import { Component, Input } from '@angular/core';
import {OrderItem} from 'src/app/orders/models/order-item';

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html'
})
export class OrderItemComponent {
  private _items: OrderItem[] = [];

  get orderItems(): OrderItem[]{
    return this._items;
  }

  @Input()
  set orderItems(value: OrderItem[]){
    this._items = value;
  }

}
