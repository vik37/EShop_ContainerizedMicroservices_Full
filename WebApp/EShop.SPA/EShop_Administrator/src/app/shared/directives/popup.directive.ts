import { Directive, ElementRef, HostListener, OnChanges, Renderer2, SimpleChanges, Input }
from '@angular/core';

@Directive({
  selector: '[appPopup]'
})
export class PopupDirective {

  @Input('appPopup')
  text: string = '';

  private _pTag: any;

  constructor(private element: ElementRef, private render: Renderer2) { }


  @HostListener('mouseover') onMouseOver(){
    if(this.text){
      this.createElement(this.text);
    }
  }
  @HostListener('mouseout') onMouseLeave(){
    if(this._pTag){
      this.cleanElement(this._pTag);
    }
  }

  createElement(text: string): void{
    this._pTag = this.render.createElement('p');
    const insertText = this.render.createText(text);
    this.render.addClass(this._pTag,'popup');
     this.render.appendChild(this._pTag,insertText);
     this.render.appendChild(this.element.nativeElement,this._pTag);
  }

  cleanElement(pTag: any): void{
    this.render.removeChild(this.element.nativeElement,pTag);
  }
}
