using Unomart.Managers;
using Unomart.DB;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Coupons
        DayOfWeek[] x = { DayOfWeek.Monday, DayOfWeek.Wednesday };
        CouponManager.coupons.Add(new CouponManager.Coupon(name: "TEST", des: "This is Manual text providing more info on coupon.\n", from: new DateOnly(), to: new DateOnly(), min: 10, days: x, redeemCount: 10, always: false, discount: 0.15f, discountDelivery: 1f));
        CouponManager.coupons.Add(new CouponManager.Coupon(name: "FREE50", des: "", min: 15, redeemCount: 1000, always: false, discount: 0.5f));
        CouponManager.coupons.Add(new CouponManager.Coupon(name: "PIZZA20", des: "Provided by Dominos!\n", redeemCount: 1000, always: true, discount: 0.2f));
        CouponManager.coupons.Add(new CouponManager.Coupon(name: "DELIFREE", des: "", redeemCount: 1000, always: false, from: new DateOnly(2023, 1, 1), to: new DateOnly(2023, 5, 1), discountDelivery: 1f));
 

        //Cache Prep
        DAL.Open();
        DAL.PrepareCache();
        //10512000 365
        var timer1 = new Timer(_ => { Console.WriteLine("Updated Currency"); CurrencyManager.Invalidate();}, null, 0, 10540800);


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        DAL.Close();
    }
}