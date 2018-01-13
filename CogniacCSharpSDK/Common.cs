using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Cogniac
{
    public static class Serialize
    {
        public static string ToJson(this object self)
            => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }

    internal class Helpers
    {
        public static string FlattenStringArray(string[] array)
        {
            string result = "[";
            foreach (string s in array)
            {
                result += "\"" + s + "\",";
            }
            result = result.TrimEnd(',');
            result += "]";
            return result;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static string MapToQueryString(IDictionary<string, object> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }

        public static byte[] GetFileBytesContent(string fileName)
        {
            byte[] sContents;
            if ((fileName.ToLower().StartsWith("https://")) || (fileName.ToLower().StartsWith("http://")))
            {
                // if URL, download it
                WebClient wc = new WebClient();
                sContents = wc.DownloadData(fileName);
            }
            else
            {
                // Get file size
                FileInfo fi = new FileInfo(fileName);
                // Disk access
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                sContents = br.ReadBytes((int)fi.Length);
                br.Close();
                fs.Close();
            }
            return sContents;
        }
    }
}
