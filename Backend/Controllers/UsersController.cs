using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Unomart.DB;
using Unomart.Managers;
using Unomart.Models;

namespace Unomart.Controllers
{
    [Route("api/User/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("Login")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult Login(string? email, string? pw)
        {
            try
            {
                if (email == null || pw == null)
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ4"));

                try { _ = new MailAddress(email); }
                catch (Exception) { return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1")); }

                var user = DAL.getUserFromEmail(email);

                if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, new Message(null, "SERVER"));

                if (user.Email == null) return StatusCode(StatusCodes.Status200OK, new Message(null, "ABSENT"));


                bool ok = user.Hash == Hash(pw, user.Email);

                return StatusCode(StatusCodes.Status200OK, new Message(ok ? SessionManager.Add(user.UID) : '0', "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }
        
        
        [HttpGet("AutoLogin")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult AutoLogin(string? SID)
        {
            try
            {
                if (SID== null)
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));


                return StatusCode(StatusCodes.Status200OK, new Message(SessionManager.isValid(SID), "WORKED"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


        [HttpGet("Get")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult GetUser(string? SID)
        {
            try {
                if (SID.Length == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));
                }

                string s = SessionManager.validityCheck(SID);

                User? u = DAL.getUser(s);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
                }

                u.UID = "REDACTED";

                return StatusCode(StatusCodes.Status200OK, new Message(u, "WORKED"));
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }
        }


        [HttpPost("Create")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult PostUser(User user, string address)
        {
            try {
                if (user.Email == null || user.Hash == null || user.Username == null || address == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ4"));
                }
                try { _ = new MailAddress(user.Email); }
                catch (Exception) { return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1")); }

                if (user.Hash.Length < 6)
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ2"));
                if (user.Username.Length < 4)
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ3"));
                user.UID = Guid.NewGuid().ToString();

                Address ad = new();
                ad.UID = user.UID;
                ad.AID = Guid.NewGuid().ToString();
                ad.UserAddress = address;
                DAL.setUser(user);
                DAL.setAddress(ad);

                
                return StatusCode(StatusCodes.Status200OK, new Message(SessionManager.Add(user.UID),"WORKED"));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "PRESNT"));
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Message), 200)]
        public IActionResult PutUser(User user, string ?newpw)
        {
            try
            {
                if (user.Email == null || user.Hash == null || user.Username == null)return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ4"));
                if (user.Username.Length < 4) return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ1"));

                if (newpw == null)
                {
                    DAL.updateUser(user, true);
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
                }

                if (newpw.Length < 6) return StatusCode(StatusCodes.Status200OK, new Message(null, "BDREQ2"));
                                
                var uA = DAL.getUserFromEmail(user.Email);
                if(uA.Hash == Hash(user.Hash, user.Email))
                {
                    user.Hash = Hash(newpw,user.Email);
                    DAL.updateUser(user);
                    return StatusCode(StatusCodes.Status200OK, new Message(null, "WORKED"));
                }

                return StatusCode(StatusCodes.Status200OK, new Message(null,"BDREQ3"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new Message(null, "SERVER"));
            }

        }


        public static string Hash(string pw, string email)
        {
            email = email.ToUpper();
            string s = pw + email;

            byte[] tmpHash = MD5.HashData(Encoding.ASCII.GetBytes(s));
            s = "";
            foreach(byte b in tmpHash)
            {
                s += b.ToString("X2");
            }
            return s;
        }
    }
}