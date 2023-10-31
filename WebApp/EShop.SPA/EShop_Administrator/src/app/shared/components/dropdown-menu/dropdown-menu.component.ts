import { Component, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'app-dropdown-menu',
  templateUrl: './dropdown-menu.component.html'
})
export class DropdownMenuComponent {
  private _buttonText: string = '';
  private _data:{id:number,name:string}[] = [];

  get buttonText(): string{
    return this._buttonText;
  };

  @Input()
  set buttonText(value:string){
    this._buttonText = value;
  }

  get data(): {id:number,name:string}[]{
    return this._data;
  }

  @Input()
  set data (value:any){
    this._data = value;
  };
}
