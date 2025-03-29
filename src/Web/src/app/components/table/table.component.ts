import { NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  imports: [NgFor, NgIf],
})
export class TableComponent {
  @Input() columns: { key: string; label: string }[] = [];
  @Input() data: any[] = [];
  @Input() pageSize: number = 5;

  currentPage: number = 1;
  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  Math = Math;

  get sortedData() {
    let sorted = [...this.data];
    if (this.sortColumn) {
      sorted.sort((a, b) => {
        let result = a[this.sortColumn] > b[this.sortColumn] ? 1 : -1;
        return this.sortDirection === 'asc' ? result : -result;
      });
    }
    return sorted.slice(
      (this.currentPage - 1) * this.pageSize,
      this.currentPage * this.pageSize
    );
  }

  get totalPages(): number {
    return Math.ceil(this.data.length / this.pageSize);
  }

  setSort(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
  }

  changePage(page: number) {
    this.currentPage = page;
  }
}
