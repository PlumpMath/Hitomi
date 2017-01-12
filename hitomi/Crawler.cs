﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hitomi
{
    public partial class Crawler : Form
    {
        public Crawler()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tag = comboBox1.Text + textBox1.Text;
            string language = textBox2.Text;
            int num = (int)numericUpDown1.Value;

            foreach(int i in Hitomi.CrawlFromTag(tag, language).Skip((int)numericUpDown2.Value).Take(num))
            {
                Manga m = new Manga(i);
                m.Download($"Download\\{i}_{m.Name}");
            }
        }
    }
}
