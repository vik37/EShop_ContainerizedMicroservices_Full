import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin, pipe } from 'rxjs';
import {map,filter} from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {OrderSummaryViewModel} from 'src/app/orders/models/order-summary';


@Injectable()
export class OrderService {
  API_HTTP: string = environment.ORDERAPI_URL+'order/admin';
  constructor(private http: HttpClient) { }

  getAllOrderSummary(): Observable<OrderSummaryViewModel[]>{
    const headers = new HttpHeaders()
                            .set('content-type','application/json')
.                           set('X-Version','1');
    return this.http.get(this.API_HTTP,{'headers':headers}).pipe(

      map(data => data as OrderSummaryViewModel[])
    )
  }
}
