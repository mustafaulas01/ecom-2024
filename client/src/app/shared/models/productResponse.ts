import { Product } from "./product";

export type ProductResponse =
{
    pageIndex:number;
    pageSize:number;
    count:number;
    data:Product[]
}