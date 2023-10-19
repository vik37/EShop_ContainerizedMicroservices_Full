import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HideAfterSomeTimeDirective } from 'src/app/shared/directives/hide-after-some-time.directive';
import { PagginationComponent } from 'src/app/shared/components/paggination/paggination.component';
import { TablesComponent } from 'src/app/shared/components/tables/tables.component';
import { CardsComponent } from 'src/app/shared/components/cards/cards.component';
import { EditBackgroundByStatusDirective } from './directives/edit-background-by-status.directive';
import { PopupDirective } from './directives/popup.directive';

import {PagginationService} from 'src/app/shared/services/paggination.service';


@NgModule({
  declarations: [
      HideAfterSomeTimeDirective,
      PagginationComponent,
      TablesComponent,
      CardsComponent,
      EditBackgroundByStatusDirective,
      PopupDirective
    ],
  imports: [
    CommonModule
  ],
  exports: [
      HideAfterSomeTimeDirective,
      EditBackgroundByStatusDirective,
      PopupDirective,
      PagginationComponent,
      TablesComponent,
      CardsComponent
  ],
  providers:[PagginationService]
})
export class SharedModule { }
