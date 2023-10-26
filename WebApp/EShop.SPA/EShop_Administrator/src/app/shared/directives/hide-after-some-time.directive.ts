import { Directive, TemplateRef, ViewContainerRef,
        Input, OnChanges, SimpleChanges,
        OnInit } from '@angular/core';

@Directive({
  selector: '[hideAfterSomeTime]'
})
export class HideAfterSomeTimeDirective implements OnChanges, OnInit{
  @Input('hideAfterSomeTime')
  isHidden?: boolean;

  @Input('hideAfterSomeTimeElse')
  anotherTemplate: TemplateRef<any> | null = null;

  private _delay: number = 1500;
  constructor(private _template: TemplateRef<any>, private _view: ViewContainerRef) { }

  ngOnInit(): void {
    this._view.createEmbeddedView(this._template);
  }

  ngOnChanges(changes: SimpleChanges): void {
    setTimeout(()=>{
      if(!this.isHidden && this.anotherTemplate){
        this._view.clear();
        this._view.createEmbeddedView(this.anotherTemplate);
      }
      else{
        this._view.clear();
        this._view.createEmbeddedView(this._template);
      }
    },this._delay);
  }
}
