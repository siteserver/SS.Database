using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SS.Database.Core
{
    public static class Utils
    {
        public const string PluginId = "SS.Database";

        public static bool StartsWithIgnoreCase(string text, string startString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startString)) return false;
            return text.Trim().ToLower().StartsWith(startString.Trim().ToLower()) ||
                   string.Equals(text.Trim(), startString.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static List<int> StringCollectionToIntList(string collection)
        {
            var list = new List<int>();
            if (!string.IsNullOrEmpty(collection))
            {
                var array = collection.Split(',');
                foreach (var s in array)
                {
                    int.TryParse(s.Trim(), out var i);
                    list.Add(i);
                }
            }

            return list;
        }

        public static string ObjectCollectionToString(ICollection collection)
        {
            var builder = new StringBuilder();
            if (collection == null) return builder.ToString();

            foreach (var obj in collection)
            {
                builder.Append(obj.ToString().Trim()).Append(",");
            }

            if (builder.Length != 0) builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;

            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }
    }
}