using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace foo.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string Ip(string id)
        {
            string pingable = "";
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(id);
                pingable = reply.Status.ToString();
            }
            catch (PingException ex)
            {
                pingable = ex.Message.ToString() + " " + ex.StackTrace.ToString();
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable.ToString();
        }
    }
}