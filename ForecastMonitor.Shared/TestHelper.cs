using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ForecastMonitor.Shared
{

    public static class TestHelper
    {
        public const string SharedProjectDirectoryName = "ForecastMonitor.Shared";

        public static string ReadJson(string fileName, string projectFolder = SharedProjectDirectoryName)
        {
            var path = Path.Combine(FindFolder(projectFolder).FullName, fileName);
            using (var file = File.OpenText(path))
            {
                var json = file.ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(json);
                json = JsonConvert.SerializeObject(data);
                return json;
            }
        }

        public static string FilterJson(string json, string key, object value)
        {
            dynamic data = JsonConvert.DeserializeObject(json);
            var enumerable = ((IEnumerable)data).Cast<dynamic>();
            var dataString = JsonConvert.SerializeObject(enumerable.Where(_ => _[key] == value));
            return dataString;
        }

        public static DirectoryInfo FindFolder(string folder)
        {
            var info = Directory.GetParent(TestContext.CurrentContext.TestDirectory);

            while (info != null && !info.Name.Equals(folder, StringComparison.InvariantCultureIgnoreCase))
            {
                if (info.GetDirectories().Any(dir => dir.Name.Equals(folder)))
                {
                    return info.GetDirectories().First(dir => dir.Name.Equals(folder));
                }
                info = info.Parent;
            }

            return info;
        }
    }
}
