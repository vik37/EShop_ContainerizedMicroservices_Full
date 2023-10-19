import { Injectable } from '@angular/core';
import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, retry, catchError, Subject, EMPTY,
        BehaviorSubject } from 'rxjs';
import {map, tap, switchMap, mergeMap} from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {PagginationOrderSummaryViewModel} from 'src/app/orders/models/paggination-order-summary';
import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';
import {Paggination} from 'src/app/shared/models/paggination';
import {PagginationService} from 'src/app/shared/services/paggination.service';

import {errorMessagesByStatus} from 'src/app/shared/helper/error-messages';


@Injectable()
export class OrderService {

  private API_HTTP: string = environment.ORDERAPI_URL;

  private headers: HttpHeaders = new HttpHeaders()
          .set('content-type','application/json')
          .set('X-Version','1');

  constructor(private http: HttpClient, private pagginationService: PagginationService) { }


latestOrder$ = this.http.get(this.API_HTTP+"latest-ordersummary",{'headers':this.headers})
                          .pipe(
                            map(data => data),
                            retry(5),
                            catchError(this.httpError)
                          );

  $currentPage = new BehaviorSubject<number>(1);
  currPage$ = this.$currentPage.asObservable();

  getOlderOrderSummary(): void{
    console.log('getOlderOrderSummary() its works');
    this.currPage$.subscribe(data => console.log('from order service', data))
    this.currPage$.pipe(
      switchMap(currentPage => {
        console.log('from mp',currentPage)
       return this.http.get(this.API_HTTP+`older-ordersummary?pageSize=5&currentPage=${currentPage}`
                                        ,{'headers':this.headers})
      .pipe(
          tap(data => console.log(data)),
          map(data => data as PagginationOrderSummaryViewModel),
          map(data => {
            //this.orderSummaryViewModelSub.next(data.data);
            console.log(data)
            return data as Paggination
          }),
          map(data => this.pagginationService.pagginationSub.next(data)),
          retry(5),
          catchError(this.httpError)
      )
      })
    )
  }

  olderOrderSummary$ = this.$currentPage.pipe(
    switchMap(current =>  this.http.get(this.API_HTTP+`older-ordersummary?pageSize=5&currentPage=${current}`
    ,{'headers':this.headers}).pipe(
      tap(data => console.log('load next page: ',data)),
      map(data => data as PagginationOrderSummaryViewModel),
      tap(data => this.pagginationService.pagginationSub.next({
          currentPage: data.currentPage,
          totalItems: data.totalItems,
          totalPages: data.totalPages,
          startItem: data.startItem,
          endItem: data.endItem,
          hasNext: data.hasNext,
          hasPrevious: data.hasPrevious
      } as Paggination)),
      map(data => data.data)
    ))
  );

  private httpError(error: HttpErrorResponse){
      error.error.message = errorMessagesByStatus(error.status,'There are no new orders in the last 2 days!');
      console.log('Someting bad happend: ',error);
    return throwError(() => error);
  }
}
