using Newtonsoft.Json.Linq;
using RestSharp;

namespace Unomart.Managers
{
    public static class CurrencyManager
    {
        public class Currency
        {
            public string code { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public  float rate { get; set; }
            public Currency(string code, string name, string symbol)
            {
                this.code = code;
                this.name = name;
                this.rate = 0;
                this.symbol = symbol;
            }
        }
        //EUR,GBP,INR,USD,AUD,CAD,SGD,MXN,JPY,RUB,CNY,CHF


        public static DateTime timestamp = new DateTime();
        public static string baseCurrency = "USD";
        private static Currency[] currencies = {
            new Currency("EUR", "Euro",             "&#8364;"),
            new Currency("GBP", "Pound Sterling",   "&#163;"),
            new Currency("INR", "Indian Rupee",     "&#8377;"),
            new Currency("USD", "US Dollar",        "&#36;"),
            new Currency("AUD", "Australian Dollar","AU&#36;"),
            new Currency("CAD", "Canadian Dollar",  "CA&#36;"),
            new Currency("SGD", "Singapore Dollar", "SG&#36;"),
            new Currency("MXN", "Mexican Peso",     "MX&#36;"),
            new Currency("JPY", "Japanese Yen",     "&#165;"),
            new Currency("RUB", "Russian Ruble",    "&#8381;"),
            new Currency("CNY", "Chinese Yuan",     "&#20803;"),
            new Currency("CHF", "Swiss Franc",      "&#8355;"),
        };

        public static float deliveryCharge = 0.5f;

        public static void Invalidate()
        {
            var client = new RestClient("https://api.apilayer.com/exchangerates_data/latest?symbols=EUR%2CGBP%2CINR%2CUSD%2CAUD%2CCAD%2CSGD%2CMXN%2CJPY%2CRUB%2CCNY%2CCHF&base=USD");

            var request = new RestRequest("", Method.Get);
            request.AddHeader("apikey", "BKQkBXVfbFoblADYmFxr7cHMsXuX9Krh");

            RestResponse response = client.Execute(request);

            SetCurrencies(response.Content + "");
        }


        public static Currency[] GetCurrencies()
        {
            return currencies;
        }
        public static void SetCurrencies(string content)
        {
            JObject a = JObject.Parse(content);
            baseCurrency = a.GetValue("base").ToString();
            timestamp = new DateTime(long.Parse(a.GetValue("timestamp").ToString()));
            foreach (var item in currencies)
            {
                item.rate = float.Parse(a.GetValue("rates").ToObject<JObject>().GetValue(item.code).ToString());
            }
        }
        public static Currency GetCurrency(string? code)
        {
            foreach (var item in currencies)
            {
                if (item.code == code)
                {
                    return item;
                }
            }

            return currencies[3];
        }
        
        public static float Convert(float val, string code)
        {
            foreach (Currency currency in currencies)
            {
                if (currency.code == code)
                {
                    return (float)(Math.Floor(val * currency.rate * 100f) / 100.0f);
                }
            }

            return val;
        }

        public static float Revert(float val, string code)
        {
            foreach (Currency currency in currencies)
            {
                if (currency.code == code)
                {
                    return (float)(Math.Floor(val / currency.rate * 100f) / 100.0f);
                }
            }

            return val;
        }


    }
}