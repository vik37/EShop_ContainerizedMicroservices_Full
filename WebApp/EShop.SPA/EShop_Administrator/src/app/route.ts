import { Routes } from "@angular/router";

import {PageNotfoundComponent} from 'src/app/core/components/notfound/pageNotFound.component';

export const routes: Routes = [
  {path: 'dashboard', loadChildren: () => import('src/app/dashboard/dashboard.module')
                                                  .then(m => m.DashboardModule)},
  {path: 'orders', loadChildren: () => import('src/app/orders/orders.module')
                                              .then(m => m.OrdersModule)},
  {path: 'catalog', loadChildren: () => import('src/app/catalog/catalog.module')
                                                .then(m => m.CatalogModule)},
  {path: 'customers', loadChildren: () => import('src/app/customers/customers.module')
                                                  .then(m => m.CustomersModule)},
  {path: 'promotions', loadChildren: () => import('src/app/promotions/promotions.module')
                                                  .then(m => m.PromotionsModule)},
  {path: '', redirectTo: '/dashboard',pathMatch: "full"},
  {path: '**', component: PageNotfoundComponent}
]
