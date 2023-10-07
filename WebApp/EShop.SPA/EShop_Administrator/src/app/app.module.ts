import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


import { AppRoutingModule } from 'src/app/app-routing.module';
import { AppComponent } from 'src/app/app.component';
import { MainComponent } from 'src/app/core/components/main/main.component';
import { NavbarComponent } from 'src/app/core/components/navbar/navbar.component';
import { PageNotfoundComponent } from 'src/app/core/components/notfound/pageNotFound.component';

import { HideAfterSomeTimeDirective } from 'src/app/shared/directives/hide-after-some-time.directive';

import {OrdersModule} from 'src/app/orders/orders.module';
import {CatalogModule} from 'src/app/catalog/catalog.module';
import {CustomersModule} from 'src/app/customers/customers.module';
import {DashboardModule} from 'src/app/dashboard/dashboard.module';
import {PromotionsModule} from 'src/app/promotions/promotions.module';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    NavbarComponent,
    PageNotfoundComponent,
    HideAfterSomeTimeDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    OrdersModule,
    CatalogModule,
    CustomersModule,
    DashboardModule,
    PromotionsModule
  ],
  exports:[HideAfterSomeTimeDirective],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
