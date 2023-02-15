export class Currency
{
    name:string = "US Dollars"
    code:string = "USD";
    symbol:string = "$";
    Set(val: Currency) {
        this.name = val.name;
        this.code = val.code;
        this.symbol = val.symbol
    }
}