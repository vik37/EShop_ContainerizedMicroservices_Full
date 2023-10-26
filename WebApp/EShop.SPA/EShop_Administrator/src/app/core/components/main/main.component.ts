import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html'
})
export class MainComponent implements OnInit{

  public bootstrapMDSize: number = 10;

  ngOnInit(): void {
    this.resizeMainSection(window.innerWidth);
  }

  @HostListener('window:resize',['$event'])
  onWindowResize(): void{
    this.resizeMainSection(window.innerWidth);
  }

  resizeMainSection(screenWidth: number): void{
    if(screenWidth < 1300)
      this.bootstrapMDSize = 12;
    else
      this.bootstrapMDSize = 10;
  }
}
