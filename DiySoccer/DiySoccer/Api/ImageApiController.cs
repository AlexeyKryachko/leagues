using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Core.Services.Medias.BuisnessLogic;
using WebApi.OutputCache.V2.TimeAttributes;

namespace DiySoccer.Api
{
    public class ImageApiController : BaseApiController
    {
        private readonly IMediaManager _mediaManager;

        public ImageApiController(IMediaManager mediaManager)
        {
            _mediaManager = mediaManager;
        }

        #region GET

        [Route("api/image/{mediaId}")]
        [CacheOutputUntil(2017, 1, 1)]
        [HttpGet]
        public HttpResponseMessage GetCutImage(string mediaId, int? width = null, int? height = null)
        {
            var model = _mediaManager.GetCutImage(mediaId, width, height);
            if (model == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(model.Stream)
            };

            // Find the MIME type
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(model.ContentType);
            return response;
        }

        [Route("api/image/uncut/{mediaId}")]
        [CacheOutputUntil(2017, 1, 1)]
        [HttpGet]
        public HttpResponseMessage GetImage(string mediaId, int? width = null, int? height = null)
        {
            var model = _mediaManager.GetImage(mediaId, width, height);
            if (model == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(model.Stream)
            };

            // Find the MIME type
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(model.ContentType);
            return response;
        }

        #endregion

        #region POST

        [Route("api/upload/logo")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            if (HttpContext.Current.Request.Files.Count != 1)
                return null;
            
            var media = _mediaManager.Upload(HttpContext.Current.Request.Files[0]);
            
            return Ok(media);
        }

        #endregion
    }
}