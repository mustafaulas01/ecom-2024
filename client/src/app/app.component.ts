import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { ProductResponse } from './shared/models/productResponse';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl='https://localhost:5001/api/';
  title = 'DOGO B2B';

  products:Product[]=[];

  constructor(private http:HttpClient) {}

  ngOnInit(): void {
   this.http.get<ProductResponse>(this.baseUrl+'products').subscribe({
    next:response=>this.products=response.data,
    error:error=>console.log(error),
    complete:()=>console.log("complete")
   })
  }
}
