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
            int from = (int)numericUpDown2.Value;
            int to = (int)numericUpDown1.Value;
            IEnumerable<int> list;
            if (comboBox2.Text == "태그")
                list = Hitomi.CrawlFromTag(tag, language);
            else if (comboBox2.Text == "시리즈")
                list = Hitomi.CrawlFromSeries(tag, language);
            else
                list = null;
            foreach (int i in list.Skip(from).Take(to - from))
            {
                Manga m = new Manga(i);
                m.Download($"Download\\{i}_{m.Name}");
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Minimum = numericUpDown2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Maximum = numericUpDown1.Value;
        }
    }
}
