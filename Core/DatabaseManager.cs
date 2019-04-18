using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Dapper;
using Datory;
using Newtonsoft.Json.Linq;
using SiteServer.Plugin;

namespace SS.Database.Core
{
    public static class DatabaseManager
    {
        public static Repository Repository => new Repository(Context.Environment.Database);

        public static void ExecuteSql(string sqlString)
        {
            if (string.IsNullOrEmpty(sqlString)) return;

            Repository.Database.Connection.Execute(sqlString);
        }

        public static List<string> GetTableNameList()
        {
            return DatoryUtils.GetTableNames(Context.Environment.Database);
        }

        public static List<TableColumn> GetTableColumnInfoList(string tableName)
        {
            return DatoryUtils.GetTableColumns(Context.Environment.Database, tableName);
        }

        public static int GetCount(string tableName)
        {
            var repository = new Repository(Context.Environment.Database, tableName);
            return repository.Count();
        }

        public static List<dynamic> GetDataInfoList(string query)
        {
            return Repository.Database.Connection.Query(query).ToList();
        }

        public static List<string> GetPropertyKeysForDynamic(dynamic dynamicToGetPropertiesFor)
        {
            var jObject = (JObject) JToken.FromObject(dynamicToGetPropertiesFor);
            Dictionary<string, object> values = jObject.ToObject<Dictionary<string, object>>();
            List<string> toReturn = new List<string>();
            foreach (string key in values.Keys)
            {
                toReturn.Add(ToCamelCase(key));
            }

            return toReturn;
        }

        private static string ToCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }

            return str;
        }

        public static string GetSecretKey()
        {
            var secretKey = string.Empty;
            try
            {
                var doc = new XmlDocument();

                var configFile = Path.Combine(Context.Environment.PhysicalApplicationPath, "Web.config");

                doc.Load(configFile);

                var appSettings = doc.SelectSingleNode("configuration/appSettings");
                if (appSettings != null)
                {
                    foreach (XmlNode setting in appSettings)
                    {
                        if (setting.Name != "add") continue;

                        var attrKey = setting.Attributes?["key"];
                        if (attrKey == null) continue;

                        if (!Utils.EqualsIgnoreCase(attrKey.Value, "SecretKey")) continue;

                        var attrValue = setting.Attributes["value"];
                        if (attrValue != null)
                        {
                            secretKey = attrValue.Value;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return secretKey;
        }

        public static void Execute(string execute)
        {
            Repository.Database.Connection.Execute(execute);
        }
    }
}
