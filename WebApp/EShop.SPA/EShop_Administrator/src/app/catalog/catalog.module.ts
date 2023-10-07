import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CatalogRoutingModule } from './catalog-routing.module';

import {AddNewProductsComponent} from 'src/app/catalog/components/add-new-products/add-new-products.component';
import {ManageProductsComponent} from 'src/app/catalog/components/manage-products/manage-products.component';

@NgModule({
  declarations: [
    AddNewProductsComponent,
    ManageProductsComponent
  ],
  imports: [
    CommonModule,
    CatalogRoutingModule
  ]
})
export class CatalogModule { }
