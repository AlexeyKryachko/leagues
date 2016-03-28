using System.Web.Http;
using System.Web.Http.Cors;

namespace DiySoccer.Api
{
    public class AuthApiController : BaseApiController
    {
        [EnableCors("*", "*", "*")]
        [Route("api/authVk")]
        [HttpGet]
        public IHttpActionResult AuthVk([FromUri]string code)
        {
            return Ok();
        }        
    }
}