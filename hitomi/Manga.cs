using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace hitomi
{
    public class Manga
    {
        public string Url { get; private set; }
        public string Number
        {
            get
            {
                var u = Url.Split('.');
                return u[u.Length - 2].Split('/').Last();
            }
        }

        public string Artist { get; private set; }
        public string Series { get; private set; }
        public string Type { get; private set; }
        public string Language { get; private set; }
        public string[] Tags { get; private set; }

        public int Length { get; private set; }

        private List<string> _images;

        public Manga(string url)
        {
            Url = url;
            InitializeInfo();
        }

        private void InitializeInfo()
        {
            _images = new List<string>();
            using (WebClient client = new WebClient())
            {
                string data = client.DownloadString(Url.Replace(".html", ".js")).Replace(",", "," + Environment.NewLine);
                Regex r = new Regex("[0-9]*\\.[png|jpg]..");
                var c = r.Matches(data);
                foreach(Match m in c)
                {
                    foreach(Capture cap in m.Captures)
                    {
                        _images.Add(cap.Value);
                    }
                }
            }
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
                Directory.CreateDirectory(path);

            using (WebClient client = new WebClient())
            {
                foreach (var s in _images)
                {
                    client.DownloadFile($"https://ba.hitomi.la/galleries/{Number}/{s}", Path.Combine(path, s));
                }
            }
        }
    }
}
