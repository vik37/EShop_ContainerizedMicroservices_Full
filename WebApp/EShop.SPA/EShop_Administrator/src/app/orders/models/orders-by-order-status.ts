export interface OrdersByOrderStatus{
  statusId: number,
  statusName: string,
  orderNumber: number,
  orderDate: Date,
  buyerName: string,
  paidBy: string,
  quantityByDifferentProduct: number,
  totalProducts: number,
  totalPrice: number
}
