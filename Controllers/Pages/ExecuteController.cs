using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Database.Core;

namespace SS.Database.Controllers.Pages
{
    [RoutePrefix("pages/execute")]
    public class ExecuteController : ApiController
    {
        private const string Route = "";

        [HttpPost, Route(Route)]
        public IHttpActionResult Execute()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin ||
                    !request.AdminPermissions.HasSystemPermissions(Utils.PluginId))
                {
                    return Unauthorized();
                }

                var execute = request.GetPostString("execute");
                var secretKey = request.GetPostString("secretKey");
                if (secretKey != DatabaseManager.GetSecretKey())
                {
                    return BadRequest("Secret Key 输入错误！");
                }

                DatabaseManager.Execute(execute);

                return Ok(new
                {
                    Value = true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}