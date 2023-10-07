import { Component } from '@angular/core';
import {environment} from 'src/environments/environment';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html'
})
export class MainComponent {
  envUrl: string = environment.GATEWAY_URL;

}
