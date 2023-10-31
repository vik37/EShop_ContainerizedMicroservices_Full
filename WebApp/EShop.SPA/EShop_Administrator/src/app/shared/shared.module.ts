import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { HideAfterSomeTimeDirective } from 'src/app/shared/directives/hide-after-some-time.directive';
import { PagginationComponent } from 'src/app/shared/components/paggination/paggination.component';
import { TablesComponent } from 'src/app/shared/components/tables/tables.component';
import { CardsComponent } from 'src/app/shared/components/cards/cards.component';
import { EditBackgroundByStatusDirective } from './directives/edit-background-by-status.directive';
import { PopupDirective } from './directives/popup.directive';

import {PagginationService} from 'src/app/shared/services/paggination.service';
import { DropdownMenuComponent } from './components/dropdown-menu/dropdown-menu.component';
import { OrderEventButtonsComponent } from './components/order-event-buttons/order-event-buttons.component';


@NgModule({
  declarations: [
      HideAfterSomeTimeDirective,
      PagginationComponent,
      TablesComponent,
      CardsComponent,
      EditBackgroundByStatusDirective,
      PopupDirective,
      DropdownMenuComponent,
      OrderEventButtonsComponent
    ],
  imports: [
    CommonModule,
    RouterModule,
    FontAwesomeModule
  ],
  exports: [
      HideAfterSomeTimeDirective,
      EditBackgroundByStatusDirective,
      PopupDirective,
      PagginationComponent,
      TablesComponent,
      CardsComponent,
      DropdownMenuComponent,
      OrderEventButtonsComponent
  ],
  providers:[PagginationService]
})
export class SharedModule { }
