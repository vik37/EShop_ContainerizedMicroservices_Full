import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { CatalogRoutingModule } from './catalog-routing.module';

import {AddNewProductsComponent} from 'src/app/catalog/components/add-new-products/add-new-products.component';
import {ManageProductsComponent} from 'src/app/catalog/components/manage-products/manage-products.component';

import {CatalogService} from 'src/app/catalog/services/catalog.service';
@NgModule({
  declarations: [
    AddNewProductsComponent,
    ManageProductsComponent
  ],
  imports: [
    CommonModule,
    CatalogRoutingModule,
    HttpClientModule
  ],
  providers:[CatalogService]
})
export class CatalogModule { }
