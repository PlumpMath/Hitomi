using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hitomi
{
    public class Page
    {
        public Hitomi Hitomi { get; private set; }
        public string Url { get; private set; }

        public string Artist => Hitomi.Artist;
        public string Series => Hitomi.Series;
        public string Type => Hitomi.Type;
        public string Language => Hitomi.Language;
        public string Tag => Hitomi.Tag;

        public Manga[] Mangas { get; private set; }
        public int Index { get; private set; }

        internal Page(Hitomi hitomi, int page)
        {
            Index = page;
        }

        private void InitializeMangas()
        {
            throw new NotImplementedException();
        }
    }
}
