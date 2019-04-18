using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Database.Core;

namespace SS.Database.Controllers.Pages
{
  [RoutePrefix("pages/tables")]
  public class TablesController : ApiController
  {
    private const string Route = "";

    [HttpGet, Route(Route)]
    public IHttpActionResult GetTableNameList()
    {
      try
      {
        var request = Context.AuthenticatedRequest;
        if (!request.IsAdminLoggin ||
            !request.AdminPermissions.HasSystemPermissions(Utils.PluginId))
        {
          return Unauthorized();
        }

        return Ok(new
        {
          Value = DatabaseManager.GetTableNameList()
        });
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    [HttpPost, Route(Route)]
    public IHttpActionResult GetTableColumnInfoList()
    {
      try
      {
        var request = Context.AuthenticatedRequest;
        if (!request.IsAdminLoggin ||
            !request.AdminPermissions.HasSystemPermissions(Utils.PluginId))
        {
          return Unauthorized();
        }

        var tableName = request.GetPostString("tableName");

        return Ok(new
        {
          Value = DatabaseManager.GetTableColumnInfoList(tableName),
          Count = DatabaseManager.GetCount(tableName)
        });
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }
  }
}