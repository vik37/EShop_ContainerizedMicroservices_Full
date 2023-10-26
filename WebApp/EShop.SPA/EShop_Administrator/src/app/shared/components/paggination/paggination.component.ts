import { Component, Output, EventEmitter, OnDestroy } from '@angular/core';
import {PagginationService} from 'src/app/shared/services/paggination.service';
import {Paggination} from 'src/app/shared/models/paggination';

@Component({
  selector: 'app-paggination',
  templateUrl: './paggination.component.html'
})
export class PagginationComponent implements OnDestroy{

  @Output()
  currentPageEmmiter: EventEmitter<number> = new EventEmitter<number>();

  private _paggination!: Paggination;

  get paggination(): Paggination{
    return this._paggination;
  }

  pages: number[] = [];

  constructor(private _pagginationService: PagginationService){}

  $paggination = this._pagginationService.$paggination.subscribe(data => {
    this.pages = Array(data.totalPages).fill(0).map((x,i)=>i);
    this._paggination = data;
  });

  getCurrentPagginationPage(pageNumbaer: number){
    this.currentPageEmmiter.emit(pageNumbaer);
  }

  ngOnDestroy(): void {
    this.$paggination.unsubscribe();
  }
}
