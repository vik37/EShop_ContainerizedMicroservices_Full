import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { Observable, pipe } from 'rxjs';
import {map} from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {CatalogType} from 'src/app/catalog/models/catalog-type';

@Injectable()
export class CatalogService {
  urlPath: string = environment.CATALOGAPI_URL+"catalog/catalogtypes";
  constructor(private http: HttpClient) { }

  getAllCatalogTypes() : Observable<CatalogType[]>{
    const headers = new HttpHeaders()
                            .set('content-type','application/json')
.                           set('X-Version','1');
    console.log('catalog environment url', environment.CATALOGAPI_URL);
    return this.http.get(this.urlPath,{'headers':headers}).pipe(
      map(data => data as CatalogType[])
    )
  }

}
