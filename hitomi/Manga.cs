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
        public string Type { get; private set; }
        public string Language { get; private set; }
        public string Series { get; private set; }
        public Tag[] Tags { get; private set; }

        public int Length => _images.Length;

        private string[] _images;
        
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
            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(client.DownloadString(Url));

                Name = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery')]//h1//a").InnerText;
                Artist = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery')]//h2//a").InnerText;
                //Group = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery-info')]//a[contains(@href,'/group/')]");
                Type = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery-info')]//tr[2]//a").InnerText;
                Language = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery-info')]//tr[3]//a").InnerText;
                Series = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery-info')]//tr[4]//a").InnerText;
                Tags = (from tag in doc.DocumentNode.SelectNodes("//ul[contains(@class, 'tags')]//li//a[contains(@href,'/tag/')]")
                        select new Tag(tag.InnerText)).ToArray();

                string data = client.DownloadString(Url.Replace(".html", ".js")).Replace(",", "," + Environment.NewLine);
                _images = (from Match m in Regex.Matches(data, "(?<=\")[\\w.]+[jpg|png](?=\")")
                           select m.Value.Split('"').Last()).ToArray();
            }
        }

        public IEnumerable<Image> GetImages()
        {
            foreach (var s in _images)
            {
                var request = WebRequest.Create($"https://ba.hitomi.la/galleries/{Number}/{s}");

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
                for(int i = 0; i < _images.Length; i++)
                {
                    client.DownloadFile($"https://ba.hitomi.la/galleries/{Number}/{_images[i]}", Path.Combine(path, _images[i]));
                    Downloaded?.Invoke(i);
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }
    }
}
