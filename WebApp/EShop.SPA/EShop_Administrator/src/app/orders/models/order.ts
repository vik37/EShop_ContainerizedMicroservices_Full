import {OrderItem} from 'src/app/orders/models/order-item';

export interface Order{
  orderNumber: number,
  orderDate: Date,
  description: string,
  country: string,
  state: string,
  street: string,
  city: string,
  zipCode: string,
  status: string,
  buyerName: string,
  totalPrice: number,
  totalProducts: number,
  maximumPrice: number,
  minimumPrice: number,
  averagePrice: number,
  orderItems: OrderItem[]
}
