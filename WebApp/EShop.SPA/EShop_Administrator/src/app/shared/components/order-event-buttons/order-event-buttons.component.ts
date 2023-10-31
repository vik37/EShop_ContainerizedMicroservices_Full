import { Component, Input, Output, EventEmitter } from '@angular/core';
import { faRectangleXmark, faTruckFast, faSquareH } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-order-event-buttons',
  templateUrl: './order-event-buttons.component.html'
})
export class OrderEventButtonsComponent {
  faRectangleXmark = faRectangleXmark;
  faTruckFast = faTruckFast;
  faSquareH = faSquareH;

  orderNumberEvent: EventEmitter<number> = new EventEmitter<number>();

  popupText: string = 'Send Order to the ';

  private _isOrderFinished: boolean = true;

  get isOrderFinished(): boolean{
    return this._isOrderFinished;
  }

  @Input()
  set isOrderFinished(value: boolean){
    this._isOrderFinished = value;
  }

  sendOrderNumber(orderNumber:any): void{
    this.orderNumberEvent.emit(orderNumber);
  }
}
