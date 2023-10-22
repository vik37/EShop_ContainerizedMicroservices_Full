import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar-smaller-screen',
  templateUrl: './navbar-smaller-screen.component.html',
  styleUrls: ['./navbar-smaller-screen.component.scss']
})
export class NavbarSmallerScreenComponent {
  mainRoutes: string[] = ["/dashboard","/orders","/catalog","/customers","/promotions"];
}
