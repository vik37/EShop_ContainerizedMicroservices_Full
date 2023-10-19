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

  delay: number = 1500;
  constructor(private template: TemplateRef<any>, private view: ViewContainerRef) { }

  ngOnInit(): void {
    this.view.createEmbeddedView(this.template);
  }

  ngOnChanges(changes: SimpleChanges): void {
    setTimeout(()=>{
      if(!this.isHidden && this.anotherTemplate){
        this.view.clear();
        this.view.createEmbeddedView(this.anotherTemplate);

      }
      else{
        this.view.clear();
        this.view.createEmbeddedView(this.template);
      }
    },this.delay);
  }
}
