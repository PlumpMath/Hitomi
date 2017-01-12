using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace hitomi
{
    public static class Hitomi
    {
        public static IEnumerable<int> UrlsFromPage(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "None");
                var data = client.DownloadString(url);
                var ms = Regex.Matches(data, "(?<=<a href=\"/galleries/)[0-9]*(?=.html\"><)");
                foreach(Match m in ms)
                {
                    yield return Convert.ToInt32(m.Value);
                }
            }
        }

        public static string MakeUrl(int page = 1, string tag = null, string artist = null, string language = "all")
        {
            string type, value, lan = language;
            if (tag != null)
            {
                type = "tag/";
                value = tag;
            }
            else if(artist != null)
            {
                type = "artist/";
                value = artist;
            }
            else
            {
                type = "";
                value = "index";
            }
            return $"https://hitomi.la/{type}{value}-{language}-{page}.html";
        }

        public static IEnumerator<int> CrawlFromTag(string tag)
        {
            throw new NotImplementedException();
        }
    }
}
