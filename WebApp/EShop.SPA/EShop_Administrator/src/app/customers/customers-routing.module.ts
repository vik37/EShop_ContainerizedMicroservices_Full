import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {AllBuyersComponent} from 'src/app/customers/components/all-buyers/all-buyers.component';
import {MessagesComponent} from 'src/app/customers/components/messages/messages.component';

const routes: Routes = [
  {path: '', component: AllBuyersComponent},
  {path: 'messages', component: MessagesComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule { }
