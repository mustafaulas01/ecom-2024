import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    CommonModule,
    FormsModule,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  private dialogService = inject(MatDialog);
  totalPageCount?: number;

  pageSize?: number = 10;
  pageIndex?: number | undefined = 1;
  pageSizeOptions = [5, 10, 15, 20];

  selectedBrands: string[] = [];
  selectedTypes: string[] = [];
  selectedSort: string = 'name';

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' },
  ];

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getproductTypes();
    this.getProducts();
  }

  handlePageEvent(event: PageEvent) {
    this.pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.getProducts();
  }

  getProducts() {
    this.shopService
      .getProducts(
        this.selectedBrands,
        this.selectedTypes,
        this.selectedSort,
        this.pageSize,
        this.pageIndex
      )
      .subscribe({
        next: (response) => {
          this.products = response.data;
          this.totalPageCount = response.count;
          this.pageIndex = response.pageIndex;
          this.pageSize = response.pageSize;
          console.log(
            'toplam data:' +
              this.totalPageCount +
              ' mevcut sayfa: ' +
              this.pageIndex +
              ' sayfadaki kayıt toplam:' +
              this.pageSize
          );
        },
        error: (error) => console.log('hata var :' + error),
      });
  }
  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];

    if (selectedOption) {
      this.selectedSort = selectedOption.value;
      this.pageIndex = 1;
      this.getProducts();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FilterDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.selectedBrands,
        selectedTypes: this.selectedTypes,
      },
    });

    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.selectedBrands = result.selectedBrands;
          this.selectedTypes = result.selectedTypes;
          //apply the filterse from service
          this.pageIndex = 1;
          this.getProducts();
        }
      },
    });
  }
}
