import { Component, OnInit, OnDestroy } from '@angular/core';
import {CatalogService} from 'src/app/catalog/services/catalog.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-manage-products',
  templateUrl: './manage-products.component.html'
})
export class ManageProductsComponent implements OnInit, OnDestroy {
  catalogTypeSubscription = new Subscription();

  constructor(private catalogService: CatalogService){}

  ngOnInit(): void {
    this.catalogTypeSubscription.add(this.catalogService.getAllCatalogTypes().subscribe(data =>{
      console.log('catalog types', data);
    }))
  }

  ngOnDestroy(): void {
    this.catalogTypeSubscription.unsubscribe();

  }
}
