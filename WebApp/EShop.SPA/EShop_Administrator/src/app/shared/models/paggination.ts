export interface Paggination{
  currentPage: number,
  totalItems: number,
  totalPages: number,
  startItem: number,
  endItem: number,
  hasPrevious: number,
  hasNext:boolean
}
