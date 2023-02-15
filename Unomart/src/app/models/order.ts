export class OrderItem{
    IID:string = '';
    OID:string = '';
    ItemAmount:number = 0;
    ItemCost?:number = 0;
    ItemImg?:string = '';
    ItemName:string = '';
}

export class Order{
    oid:string = '';
    uid:string = '';
    discount:number = 0;
    deliveryCharge:number = 0;
    deliveryAddress:string = '';
    total:number = 0;
    items:number = 0;
    dateTime:Date = new Date();
}