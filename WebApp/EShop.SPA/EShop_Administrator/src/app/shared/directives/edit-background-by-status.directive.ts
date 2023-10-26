import { Directive, Input, ElementRef, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[appEditBackgroundByStatus]'
})
export class EditBackgroundByStatusDirective implements OnChanges {

  private _statusColors: {[key: string]: string} = {
    submitted:"#8f8106",
    awaitingvalidation:"#a832a8",
    stockconfirmed:"#2f8508",
    paid:"#088085",
    shipped:"#a65407",
    cancelled:"#a60707"
  }

  constructor(private element: ElementRef) { }

  @Input('appEditBackgroundByStatus')
    status:string = "";

    ngOnChanges(changes: SimpleChanges): void {
      const currentChange = changes["status"];
      if(currentChange){
        this.editStatusBackground(this._statusColors[currentChange.currentValue.toLowerCase()]);
      }
    }

    editStatusBackground(color:string): void{
      this.element.nativeElement.style.backgroundColor = color;
    }
}
