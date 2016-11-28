using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hitomi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadPage(null);
        }

        public void LoadPage(Page page)
        {
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(140/3, 220/3);

            if (page != null)
            {
                foreach (Manga m in page.Mangas)
                {
                    imgs.Images.Add(m.First());
                }
            }
            this.cListView1.SmallImageList = imgs;
        } 
    }
}
