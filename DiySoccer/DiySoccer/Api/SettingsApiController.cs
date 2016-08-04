using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Interfaces.Authenticate.BuisnessLogic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.OutputCache.Core.Time;
using WebGrease.Css.Extensions;

namespace DiySoccer.Api
{
    public class SettingsApiController : BaseApiController
    {
        private readonly IAuthenticateManager _authenticateManager;

        public SettingsApiController(IAuthenticateManager authenticateManager)
        {
            _authenticateManager = authenticateManager;
        }

        #region GET

        [Route("api/settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var model = _authenticateManager.GetSettings();
            return Json(model);
        }

        /*[Route("api/templates")]
        [HttpGet]
        public IHttpActionResult GetTemplates()
        {
            var path = HttpContext.Current.Server.MapPath("/Content/dist/templates/templates.json");
            if (File.Exists(path))
            {
                var stream = File.ReadAllText(path);
                return Ok(stream);
            }

            var templates = new List<FileInfo>();
            var searchDirectory = HttpContext.Current.Server.MapPath("/Content/js");
            GetTemplateFiles(searchDirectory, templates);

            var result = new JArray();
            templates.ForEach(x =>
            {
                var token = ne
            });
            
            result.Add();

            return Json(model);
        }*/

        private void GetTemplateFiles(string parentPath, List<FileInfo> result)
        {
            var files = Directory.GetFiles(parentPath)
                .Select(x => new FileInfo(x))
                .Where(x => x.Extension == ".html");
            result.AddRange(files);

            var directories = Directory.GetDirectories(parentPath);
            directories.ForEach(x => GetTemplateFiles(x, result));
        }

        #endregion

    }
}