import { Component } from '@angular/core';

@Component({
  selector: 'app-view-order',
  templateUrl: './view-order.component.html'
})
export class ViewOrderComponent{
  orderSummaryByTimeSelection:string [] = ['latest','older'];
  orderSummaryByTimeSelected:string = this.orderSummaryByTimeSelection[0];

  fadeAnimations: string[] = ['fade-in-animation','fade-out-animation'];

  latestOrderAnimation: string = '';
  olderOrderAnimation: string = '';

  buttonStyles = [
    {'background-color':'rgb(108, 99, 135)','cursor':'text',disable:true},
    {'background-color':'rgb(30, 6, 103)','cursor':'pointer',disable:false}
  ];

  orderSummaryToggle(index:number): void{
    if(this.buttonStyles[index].disable)
      return;

    this.latestOrderAnimation = this.fadeAnimations[index];
    this.olderOrderAnimation = index === this.fadeAnimations.length-1?this.fadeAnimations[0]:this.fadeAnimations[1];
    this.buttonStyles = [this.buttonStyles[1],this.buttonStyles[0]];

    setTimeout(()=>{
      this.orderSummaryByTimeSelected = this.orderSummaryByTimeSelection[index];
    },1800);
  }
}
