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
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.UserAgent, "None");
                var data = client.DownloadString(url);
                var ms = Regex.Matches(data, "(?<=<a href=\"/galleries/)[0-9]*(?=.html\"><)");
                foreach(Match m in ms)
                {
                    yield return Convert.ToInt32(m.Value);
                }
            }
        }

        public static int MaxPage(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return Convert.ToInt32(Regex.Match(client.DownloadString(url), "(?<='.html', [0-9]*, )[0-9]*(?=\\);)").Value);
            }
        }

        public static string MakeUrl(int page = 1, string tag = null, string artist = null, string series = null, string language = "all")
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
            else if(series != null)
            {
                type = "series/";
                value = series;
            }
            else
            {
                type = "";
                value = "index";
            }
            return $"https://hitomi.la/{type}{value}-{language}-{page}.html";
        }

        public static IEnumerable<int> CrawlFromTag(string tag, string language = "all")
        {
            int max = MaxPage(MakeUrl(tag: tag, language: language));
            for (int i = 1; i <= max; i++)
            {
                string url = MakeUrl(page: i, tag: tag, language: language);
                foreach (int m in UrlsFromPage(url))
                    yield return m;
            }
        }

        public static IEnumerable<Manga> CrawlFromTag(string[] include, string[] exclude, string language = "all")
        {
            int max = MaxPage(MakeUrl(tag: include[0], language: language));
            for (int i = 1; i <= max; i++)
            {
                string url = MakeUrl(page: i, tag: include[0], language: language);
                foreach (int j in UrlsFromPage(url))
                {
                    Manga m = new Manga(j);
                    if (include.All(s => m.Tags.Contains(new Tag(s))))
                        if (!exclude.All(s => m.Tags.Contains(new Tag(s))))
                            yield return m;
                }
            }
        }

        public static IEnumerable<int> CrawlFromSeries(string series, string language = "all")
        {
            int max = MaxPage(MakeUrl(series: series, language: language));
            for (int i = 1; i <= max; i++)
            {
                string url = MakeUrl(page: i, series: series, language: language);
                foreach (int m in UrlsFromPage(url))
                    yield return m;
            }
        }
    }
}
