import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';
import {Paggination} from 'src/app/shared/models/paggination';

export interface PagginationOrderSummaryViewModel extends Paggination{
  data: OrderSummaryViewModel[]
}
