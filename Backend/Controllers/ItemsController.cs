using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.Managers;
using Unomart.DB;

namespace Unomart.Controllers
{
    [Route("api/Item/")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet("GetItems")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetItems(string? SID, string? query, string? page)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            if (query == null)
                query = "";

            if (page == null)
                page = "0";
            string s = SessionManager.validityCheck(SID);


            try
            {
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.ItemMaster.getItems(DAL.getUser(s).CurrencyCode, query, int.Parse(page)),"WORKED"));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

        [HttpGet("GetOrderItems")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetOrderItems(string? OID, string? SID)
        {
            if (OID.Length == 0 || SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }

            string s = SessionManager.validityCheck(SID);

            try
            {
                var list = DAL.getOrders(s);
                foreach (var item in list)
                {
                    if (item.OID == OID)
                    {
                        return StatusCode(StatusCodes.Status200OK, value: new Message(DAL.getOrderItems(OID, DAL.getUser(s).CurrencyCode),"WORKED"));
                    }
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
            return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));

        }
    }
}
    