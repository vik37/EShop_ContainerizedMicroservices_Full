import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html'
})
export class TablesComponent {
  private _items: any[] = [];

  get items(): any[]{
    return this._items;
  }

  @Input()
  set items(value: any[]){
    
  }
}
