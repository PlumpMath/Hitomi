using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace hitomi
{
    public class Manga
    {
        public event Action<int> Downloaded;
        public string Url { get; private set; }
        public string Number
        {
            get
            {
                var u = Url.Split('.');
                return u[u.Length - 2].Split('/').Last();
            }
        }

        public string Name { get; private set; }
        public string Artist { get; private set; }
        public string Series { get; private set; }
        public string Type { get; private set; }
        public string Language { get; private set; }
        public string[] Tags { get; private set; }

        public int Length => _images.Count;

        private List<string> _images;
        
        public Manga(string url)
        {
            Url = url;
            InitializeInfo();
        }

        public Manga(int number)
        {
            Url = $"https://hitomi.la/galleries/{number}.html";
            InitializeInfo();
        }

        private void InitializeInfo()
        {
            _images = new List<string>();
            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(client.DownloadString(Url));
                doc.DocumentNode.SelectNodes("//li//a[contains('href','tag')]");
            }

            /*
            Regex r = new Regex("(?<=\")[\\w.]+[jpg|png](?=\")");
            var c = r.Matches(data);
            foreach (Match m in c)
            {
                foreach (Capture cap in m.Captures)
                {
                    _images.Add(cap.Value.Split('"').Last());
                }
            }

            string data2 = client.DownloadString(Url);
            Name = Regex.Match(data2, "(?<=<title>)(.*)(?=Read Online)").Value.Trim(' ', '-');
            */
        }

        public IEnumerable<Image> GetImages()
        {
            foreach (var s in _images)
            {
                var request = WebRequest.Create($"https://g.hitomi.la/galleries/{Number}/{s}");

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    yield return Image.FromStream(stream);
                }
            }
        }
       
        public void Download(string path)
        {
            if (!Directory.Exists(path))
            {
                foreach (char c in Path.GetInvalidPathChars())
                    path = path.Replace(c, '_');
                Directory.CreateDirectory(path);
            }

            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.UserAgent, "None");
                foreach (var s in _images)
                {
                    client.DownloadFile($"https://ba.hitomi.la/galleries/{Number}/{s}", Path.Combine(path, s));
                    Downloaded?.Invoke(_images.IndexOf(s));
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }
    }
}
