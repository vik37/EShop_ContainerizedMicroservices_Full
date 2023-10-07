import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomersRoutingModule } from './customers-routing.module';

import {AllBuyersComponent} from 'src/app/customers/components/all-buyers/all-buyers.component';
import {MessagesComponent} from 'src/app/customers/components/messages/messages.component';

@NgModule({
  declarations: [
    AllBuyersComponent,
    MessagesComponent
  ],
  imports: [
    CommonModule,
    CustomersRoutingModule
  ]
})
export class CustomersModule { }
