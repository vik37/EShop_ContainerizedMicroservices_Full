import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import {PagginationService} from 'src/app/shared/services/paggination.service';
import {Paggination} from 'src/app/shared/models/paggination';

@Component({
  selector: 'app-paggination',
  templateUrl: './paggination.component.html'
})
export class PagginationComponent implements OnInit, OnDestroy{

  @Output()
  currentPageEmmiter: EventEmitter<number> = new EventEmitter<number>();

  private _paggination!: Paggination;

  get paggination(): Paggination{
    return this._paggination;
  }

  pages: number[] = [];

  $paggination = this.pagginationService.$paggination.subscribe(data => {
    console.log('paggination data: ',data);
    this.pages = Array(data.totalPages).fill(0).map((x,i)=>i);
    console.log('number of pages', this.pages);
    this._paggination = data;
  });


  constructor(private pagginationService: PagginationService){}

  ngOnInit(): void {

  }

  getCurrentPagginationPage(pageNumbaer: number){
    this.currentPageEmmiter.emit(pageNumbaer);
    console.log(pageNumbaer)
  }
  ngOnDestroy(): void {
    this.$paggination.unsubscribe();
  }
}
