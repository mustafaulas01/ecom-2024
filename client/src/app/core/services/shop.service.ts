import { inject, Injectable } from '@angular/core';
import { ProductResponse } from '../../shared/models/productResponse';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReturnStatement } from '@angular/compiler';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  private http = inject(HttpClient);

  types: string[] = [];
  brands: string[] = [];


  getProduct(id:number)
  {
   return this.http.get<Product>(this.baseUrl+'products/'+id);
  }

  getProducts(brands?: string[], types?: string[],sort?:string,search?:string, pageSize?:number,pageIndex?:number) {
    let params = new HttpParams();
    if (brands && brands.length > 0) {
      params = params.append('brand', brands.join(','));
    }
    if (types && types.length > 0) {
      params = params.append('type', types.join(','));
    }
    if(sort)
    {
      params=params.append("sort",sort);
    }
    if(search)
      params=params.append("search",search);

    if(pageSize)
     params=params.append('pageSize',pageSize);
   

    if(pageIndex)
    params=params.append('pageIndex',pageIndex);

    return this.http.get<ProductResponse>(this.baseUrl + 'products',{params});
  }

  getBrands() {
    if (this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: (response) => (this.brands = response),
    });
  }
  getproductTypes() {
    if (this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: (response) => (this.types = response),
    });
  }
}
