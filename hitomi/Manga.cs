using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net;
using System.IO;

namespace hitomi
{
    public class Manga : IEnumerable<Image>
    {
        public string Url { get; private set; }

        public string Artist { get; private set; }
        public string Series { get; private set; }
        public string Type { get; private set; }
        public string Language { get; private set; }
        public string[] Tags { get; private set; }

        public int Length { get; private set; }

        private string[] _imageUrls;

        public Manga(string url)
        {
            Url = url;
            InitializeInfo();
        }

        private void InitializeInfo()
        {
            using (WebClient client = new WebClient())
            {
                string data = client.DownloadString(Url);

                // TODO: 파싱해서 프로퍼티 값 설정하기
            }
        }

        public void Download(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            int i = 1;
            foreach(var img in this)
            {
                img.Save(i.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                i++;
            }
        }

        public IEnumerator<Image> GetEnumerator()
            => new MangaEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private class MangaEnumerator : IEnumerator<Image>
        {
            private Manga _manga;
            private int _current;
            private WebClient _client;
            public MangaEnumerator(Manga manga)
            {
                _manga = manga;
                _current = -1;
                _client = new WebClient();
            }

            public bool MoveNext()
            {
                if(_current < _manga.Length - 1)
                {
                    _current++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = -1;
            }

            public void Dispose()
            {
                _client.Dispose();
            }

            object IEnumerator.Current
                => Current;

            public Image Current
            {
                get
                {
                    if (_current == -1)
                        return null;

                    var data = _client.DownloadData(_manga._imageUrls[_current]);
                    using (var m = new MemoryStream())
                    {
                        return Image.FromStream(m);
                    }
                }
            }
        }
    }
}
