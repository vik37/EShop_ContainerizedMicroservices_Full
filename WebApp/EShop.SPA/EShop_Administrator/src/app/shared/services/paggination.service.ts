import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import {Paggination} from 'src/app/shared/models/paggination';

@Injectable()
export class PagginationService {
  pagginationSub = new Subject<Paggination>();
  $paggination = this.pagginationSub.asObservable();
  constructor() { }
}
