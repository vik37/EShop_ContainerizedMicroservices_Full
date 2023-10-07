import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PromotionsRoutingModule } from 'src/app/promotions/promotions-routing.module';

import {CouponsComponent} from 'src/app/promotions/components/coupons/coupons.component';
import {SpecialOffersComponent} from 'src/app/promotions/components/special-offers/special-offers.component';


@NgModule({
  declarations: [
    CouponsComponent,
    SpecialOffersComponent
  ],
  imports: [
    CommonModule,
    PromotionsRoutingModule
  ]
})
export class PromotionsModule { }
