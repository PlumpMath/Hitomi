using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace hitomi
{
    public class Hitomi
    {
        public string Artist { get; set; }
        public string Series { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string Tag { get; set; }

        public int Pages { get; private set; }

        public Hitomi(string artist = "", string series = "", string type = "", string language = "", string tag = "")
        {
            Artist = artist;
            Series = series;
            Type = type;
            Language = language;
            Tag = tag;
        }

        public Page GetPage(int page)
        {
            if (Pages < page)
                throw new IndexOutOfRangeException("page");
            return new Page(this, page);
        }

        protected static string GetUrl(string artist = "", string series = "", string type = "", string language = "", string tag = "", int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
