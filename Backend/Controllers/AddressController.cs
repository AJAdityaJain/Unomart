using Microsoft.AspNetCore.Mvc;
using Unomart.Models;
using Unomart.Managers;
using Unomart.DB;

namespace Unomart.Controllers
{
    [Route("api/Address/")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpGet("GetAddress")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetAddress(string? SID)
        {
            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }
            try
            {
                string s = SessionManager.validityCheck(SID);
                return StatusCode(StatusCodes.Status200OK, new Message(DAL.getAddress(s),"WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

        [HttpPost("CreateAddress")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult SetAddress(string SID, string? Address)
        {
            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null,"BDREQ1"));
            }
            try
            {

                string s = SessionManager.validityCheck(SID);
                Address add = new Address();

                add.UID = s;
                add.AID = Guid.NewGuid().ToString();
                add.UserAddress = Address;

                DAL.setAddress(add);

                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }

        [HttpPut("UpdateAddress")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult UpdateAddress(string SID, string AID, string? address)
        {
            if (AID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }
            if (SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ2"));
            }
            try
            {
                string s = SessionManager.validityCheck(SID);
                if (s != null)
                {
                    DAL.updateAddress(AID, address);
                }
                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        [HttpDelete("DeleteAddress")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult DeleteAddress(string AID, string SID)
        {
            if (AID.Length == 0 || SID.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
            }
            try
            {
                string s = SessionManager.validityCheck(SID);
                DAL.removeAddress(AID, s);

                return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }



    }
}
