import { Component } from '@angular/core';
import {faLinkedinIn} from '@fortawesome/free-brands-svg-icons';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'EShop_Administrator';
  faLinkedinIn = faLinkedinIn;
}
