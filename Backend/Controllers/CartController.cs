using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.DB;
using Unomart.Managers;

namespace Unomart.Controllers
{
    [Route("api/Cart/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet("GetTotal")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetTotal(string? SID)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

            try
            {
                string s = SessionManager.validityCheck(SID);
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.getTotal(s, DAL.getUser(s).CurrencyCode), "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        
        
        [HttpGet("Get")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult Get(string? SID)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            try
            {
                string s = SessionManager.validityCheck(SID);
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.getCart(s,DAL.getUser(s).CurrencyCode), "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


        [HttpGet("GetItem")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetItem(string? SID, string Item)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

            try
            {
                string s = SessionManager.validityCheck(SID);
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.getAmount(s, Item), "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


        [HttpPut("SetItem")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult UpdateCart(string? SID, string Item, int Value)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

            try
            {
                string s = SessionManager.validityCheck(SID);
                DAL.updateCartItem(s, Item, Value);
                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


        [HttpDelete("EmptyCart")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult Empty(string? SID)
        {
            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

            try
            {
                string s = SessionManager.validityCheck(SID);
                DAL.removeCart(s);
                return StatusCode(StatusCodes.Status200OK, new Message(null,"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

    }
}
