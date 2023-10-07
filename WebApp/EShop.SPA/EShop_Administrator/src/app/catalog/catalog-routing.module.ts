import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {AddNewProductsComponent} from 'src/app/catalog/components/add-new-products/add-new-products.component';
import {ManageProductsComponent} from 'src/app/catalog/components/manage-products/manage-products.component';

const routes: Routes = [
  {path: '', component: ManageProductsComponent},
  {path: 'add-product', component: AddNewProductsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CatalogRoutingModule { }
