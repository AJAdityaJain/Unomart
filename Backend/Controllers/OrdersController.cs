using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.DB;
using Unomart.Managers;

namespace Unomart.Controllers
{
    [Route("api/Order/")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet("Get")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetOrder(string? SID)
        {
            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }

            string s = SessionManager.validityCheck(SID);

            try
            {
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.getOrders(s),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        
        [HttpGet("GetDeliveryCharge")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetDeliveryCharge(string? SID)
        {
            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }

            string s = SessionManager.validityCheck(SID);

            try
            {
                return StatusCode(StatusCodes.Status200OK, new Message(CurrencyManager.Convert(CurrencyManager.deliveryCharge, DAL.getUser(s).CurrencyCode),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult PostOrder(string? SID, string? address, string? coupon)
        {
            if (coupon != null)
                coupon = coupon.ToUpper();
            string s = SessionManager.validityCheck(SID);

            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

            if (address.Length == 0 || s == null)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));

            //try
            {
                float cost = 0.0f;
                int items = 0;
                string OID = Guid.NewGuid().ToString();
                CartItem[] ci = DAL.getCart(s,DAL.getUser(s).CurrencyCode);

                foreach (var item in ci)
                {
                    items += item.ItemAmount;
                    cost += DAL.ItemMaster.getItemPrice(item.Item.ItemName) * item.ItemAmount;
                }

                Order order = new();
                order.OID = OID;
                order.UID = s;
                order.DeliveryAddress = address;
                order.Total = cost;
                order.Items = items;
                order.DeliveryCharge = CurrencyManager.deliveryCharge;

                foreach (CouponManager.Coupon co in CouponManager.coupons)
                {
                    if (co.name == coupon)
                    {
                        var sc = co.Use(cost);
                        order.Discount = sc[0];
                        order.DeliveryCharge = sc[1];
                    }
                }

                DAL.setOrder(order);

                foreach (var item in ci)
                {
                    OrderItem oi = new();
                    oi.ItemName = item.Item.ItemName;
                    oi.ItemAmount = item.ItemAmount;
                    oi.OID = OID;
                    oi.ItemCost = DAL.ItemMaster.getItemPrice(item.Item.ItemName) * item.ItemAmount;
                    DAL.setOrderItem(oi);
                }

                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            //catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


    }
}
