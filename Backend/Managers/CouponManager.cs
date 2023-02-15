namespace Unomart.Managers
{
    public static class CouponManager
    {
        public class Coupon
        {
            public bool valid { get; set; }

            public string name { get; set; }

            public int redeemCount { get; set; }

            public string desc { get; set; }
            public string description{ get; set; }

            public DateOnly? from { get; set; }
            public DateOnly? to { get; set; }
            public DayOfWeek[]? days { get; set; }
            public float? minSize { get; set; }
            public bool always { get; set; }

            public float deliveryCharge { get; set; }
            public float discount { get; set; }

            public Coupon(int redeemCount,string des,string name,bool always = false,float discountDelivery = 0.0f,float discount = 0.0f,DateOnly? from = null,DateOnly? to = null,DayOfWeek[]? days = null,float? min = null)
            {
                this.name = name;
                this.redeemCount = redeemCount;

                if (from != null)
                    this.from = (DateOnly)from;
                if (to != null)
                    this.to = (DateOnly)to;


                if (days != null)
                    this.days = days;

                this.always = always;

                if (days != null)
                    this.days = days;

                this.redeemCount = redeemCount;
                deliveryCharge = (1 - discountDelivery) * CurrencyManager.deliveryCharge;
                this.discount = discount;
                minSize = min;
                desc = des;
                stringify("USD");
            }

            public string stringify(string code)
            {
                string r = desc + " ";


                if (!always)
                {
                    if (from != null && to != null)
                        r += "Applicable from " + from.ToString() + " to " + to.ToString() + ". \n";
                    if (minSize != null)
                        r += "Order must be greater than $Currency$ " + CurrencyManager.Convert((float)minSize,code) + ". \n";
                    if (days != null && days.Length != 0)
                    {
                        int i = 0;
                        r += "Applicable only on ";
                        foreach (var day in days)
                        {
                            r += day.ToString();

                            if (i == days.Length - 2)
                            {
                                r += " and ";
                            }
                            else if (i != days.Length - 1)
                            {
                                r += ", ";
                            }

                            i++;
                        }

                        r += ". \n";
                    }
                }
                else
                {
                    r += "Can be used anytime you want! \n";
                }
                if (discount != 0)
                    r += Math.Floor(discount * 100) + "% Discount! \n";
                if (deliveryCharge == 0)
                {
                    r += "Free Delivery! \n";
                }
                else if (deliveryCharge > CurrencyManager.deliveryCharge)
                {
                    r += "Reduced delivery charges! \n";
                }
                r = r.Remove(r.Length - 1, 1);

                return r;
            }

            public float[] Use(float d)
            {
                if (Validate(d))
                {
                    redeemCount--;
                    return new float[] { discount, deliveryCharge };
                }

                return new float[] { 1.0f, 1.0f };
            }

            public bool Validate(float d)
            {
                bool r = true;
                if (minSize != null && minSize > d)
                    r = false;

                return Validate() && r;
            }
            private bool Validate()
            {
                if (redeemCount == 0)
                    return false;

                if (always)
                    return true;

                DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                DayOfWeek today = now.DayOfWeek;

                if (to != null && from != null)
                    if (now < to && now > from)
                        return true;

                if (days != null)
                    foreach (var day in days)
                    {
                        if (day == today)
                            return true;
                    }

                return true;
            }

        }

        public static List<Coupon> coupons = new List<Coupon>();

        public static object? Get(string query, float cost, string code)
        {
            foreach (var coupon in coupons)
            {
                if (coupon.name == query)//fix coupon with univ price
                {
                    if (coupon.redeemCount > 0)
                    {
                        coupon.valid = coupon.Validate(CurrencyManager.Revert(cost, code));
                        coupon.description = coupon.stringify(code);
                        return coupon;
                    }
                }
            }

            return null;
        }
    }
}
