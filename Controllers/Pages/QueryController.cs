using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Database.Core;

namespace SS.Database.Controllers.Pages
{
    [RoutePrefix("pages/query")]
    public class QueryController : ApiController
    {
        private const string Route = "";

        [HttpPost, Route(Route)]
        public IHttpActionResult Query()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin ||
                    !request.AdminPermissions.HasSystemPermissions(Utils.PluginId))
                {
                    return Unauthorized();
                }

                var query = request.GetPostString("query");
                if (!Utils.StartsWithIgnoreCase(query, "SELECT"))
                {
                    return BadRequest("请输入有效的查询SQL语句！");
                }

                var dataInfoList = DatabaseManager.GetDataInfoList(query);
                List<string> properties = null;
                var count = 0;
                if (dataInfoList != null && dataInfoList.Count > 0)
                {
                    var dataInfo = dataInfoList[0];
                    properties = DatabaseManager.GetPropertyKeysForDynamic(dataInfo);
                    count = dataInfoList.Count;
                }

                return Ok(new
                {
                    Value = dataInfoList,
                    Properties = properties,
                    Count = count
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}