using System.Collections.Generic;
using SiteServer.Plugin;

namespace SS.Database
{
    public class Plugin : PluginBase
    {
        public override void Startup(IService service)
        {
            service
                .AddSystemMenu(() => new Menu
                {
                    Text = "数据库管理",
                    IconClass = "fa fa-database",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "表结构查看器",
                            Href = "pages/tables.html"
                        },
                        new Menu
                        {
                            Text = "SQL语句查询",
                            Href = "pages/query.html"
                        },
                        new Menu
                        {
                            Text = "执行SQL命令",
                            Href = "pages/execute.html"
                        }
                    }
                })
                ;
        }
    }
}