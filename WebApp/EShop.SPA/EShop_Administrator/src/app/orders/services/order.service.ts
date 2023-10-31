import { Injectable } from '@angular/core';
import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { throwError, retry, catchError,
        BehaviorSubject, Observable } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import {PagginationOrderSummaryViewModel} from 'src/app/orders/models/paggination-order-summary';
import {Paggination} from 'src/app/shared/models/paggination';
import {Order} from 'src/app/orders/models/order';
import {OrderStatus} from 'src/app/orders/models/order-status';
import {OrdersByOrderStatus} from 'src/app/orders/models/orders-by-order-status';

import {PagginationService} from 'src/app/shared/services/paggination.service';

import {errorMessagesByStatus} from 'src/app/shared/helper/error-messages';

@Injectable()
export class OrderService {

  private API_HTTP: string = environment.ORDERAPI_URL;

  private _headers: HttpHeaders = new HttpHeaders()
                    .set('content-type','application/json')
                    .set('X-Version','1');

  constructor(private _http: HttpClient, private _pagginationService: PagginationService) { }

  latestOrder$ = this._http.get(this.API_HTTP+"latest-ordersummary",{'headers':this._headers})
                            .pipe(
                              map(data => data),
                              retry(5),
                              catchError(this.httpError)
                            );

  currentPageSub = new BehaviorSubject<number>(1);
  currentPage$ = this.currentPageSub.asObservable();

  olderOrderSummary$ = this.currentPageSub.pipe(
    switchMap(current =>  this._http
        .get(this.API_HTTP+`older-ordersummary?pageSize=5&currentPage=${current}`
    ,{'headers':this._headers}).pipe(
      map(data => data as PagginationOrderSummaryViewModel),
      tap(data => this._pagginationService.pagginationSub.next({
          currentPage: data.currentPage,
          totalItems: data.totalItems,
          totalPages: data.totalPages,
          startItem: data.startItem,
          endItem: data.endItem,
          hasNext: data.hasNext,
          hasPrevious: data.hasPrevious
      } as Paggination)),
      map(data => data.data),
      retry(5),
      catchError(this.httpError)
    ))
  );

  orderStatus$ = this._http.get(this.API_HTTP+"status")
                    .pipe(
                      map(data => data as OrderStatus[]),
                      retry(5),
                      catchError(this.httpError)
                    );

  getOrderByOrderNumber(orderNumber: string): Observable<Order>{
    return this._http.get(this.API_HTTP+`${orderNumber}`,
                {'headers':this._headers})
              .pipe(
                map(data => data as Order),
                retry(5),
                catchError(this.httpError)
              );
  }

  getOrdersByOrderStatus(orderStatusId:string): Observable<OrdersByOrderStatus[]>{
    return this._http.get(this.API_HTTP+`status/${orderStatusId}`,{'headers':this._headers})
                        .pipe(
                          map(data => data as OrdersByOrderStatus[]),
                          retry(5),
                          catchError(this.httpError)
                        );
  }

  private httpError(error: HttpErrorResponse){
      error.error.message = errorMessagesByStatus(error.status,'There are no new orders in the last 2 days!');
      console.log('Someting bad happend: ',error);
    return throwError(() => error);
  }
}
