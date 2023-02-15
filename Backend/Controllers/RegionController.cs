using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.Managers;
using Unomart.DB;

namespace Unomart.Controllers
{
    [Route("api/Region/")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        [HttpGet("Get")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetCurrencies()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Message(CurrencyManager.GetCurrencies(),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        [HttpGet("GetCurrency")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetCurrency(string SID)
        {

            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }
            string s = SessionManager.validityCheck(SID);
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Message(CurrencyManager.GetCurrency(DAL.getUser(s).CurrencyCode),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult PutCurrencies(string? SID, string? CurrencyCode)
        {

            if (SID.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            if (CurrencyCode.Length == 0)
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ2"));
            string s = SessionManager.validityCheck(SID);

            try
            {
                DAL.updateCurrency(s,CurrencyCode);
                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

    }
}
