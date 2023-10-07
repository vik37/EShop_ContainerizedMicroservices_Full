import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {CouponsComponent} from 'src/app/promotions/components/coupons/coupons.component';
import {SpecialOffersComponent} from 'src/app/promotions/components/special-offers/special-offers.component';

const routes: Routes = [
  {path: '', component: CouponsComponent},
  {path: 'special-offers', component: SpecialOffersComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PromotionsRoutingModule { }
