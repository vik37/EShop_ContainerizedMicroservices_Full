import { Component } from '@angular/core';
import {faBars, faChartSimple, faChartLine, faCartArrowDown, faRectangleList, faTags}
        from '@fortawesome/free-solid-svg-icons';
import {faAddressBook} from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {
  faBars = faBars;
  faChartSimple = faChartSimple;
  isNavbarWide: boolean = true;
  fadeInAnimation:string = '';

  navbarIcons = [faChartLine,faCartArrowDown, faRectangleList, faAddressBook,faTags];
  mainRoutes: string[] = ["/dashboard","/orders","/catalog","/customers","/promotions"];

  barsIconsIsChanged(): void{
    this.isNavbarWide = !this.isNavbarWide;
    this.fadeInAnimation = 'fade-in-animation';
  }
}
