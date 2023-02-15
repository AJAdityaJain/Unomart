using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.Managers;
using RestSharp;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Unomart.DB;
using Newtonsoft.Json.Linq;

namespace Unomart.Controllers
{
    [Route("api/Coupon/")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        [HttpGet("Get")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetCoupon(string SID,string? query)
        {
            if (query == null)
                query = "";

            try
            {
                string s = SessionManager.validityCheck(SID);
                string c = DAL.getUser(s).CurrencyCode;
                float f = DAL.getTotal(s, c).total;
                return StatusCode(StatusCodes.Status200OK, new Message(CouponManager.Get(query.ToUpper(), f,c),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

    }
}
