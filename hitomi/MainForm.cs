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
    public partial class MainForm : Form
    {
        private readonly string[] _langs = new string[]
        {
            "all",
"indonesian",
"catalan",
"czech",
"danish",
"german",
"estonian",
"english",
"spanish",
"esperanto",
"french",
"italian",
"hungarian",
"dutch",
"norwegian",
"polish",
"portuguese",
"romanian",
"albanian",
"slovak",
"finnish",
"swedish",
"tagalog",
"vietnamese",
"turkish",
"greek",
"russian",
"ukrainian",
"hebrew",
"arabic",
"thai",
"korean",
"chinese",
"japanese"
        };
        public MainForm()
        {
            InitializeComponent();

            comboBox2.SelectedIndex = 0; //태그
            comboBox1.SelectedIndex = 31; //한국어
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if(f.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = f.SelectedPath;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(listView1.SelectedItems.Count == 1)
            {
                ToolStripMenuItem item = new ToolStripMenuItem("미리보기", ((Manga)listView1.SelectedItems[0].Tag).GetImages().First());
                contextMenuStrip1.Items.Clear();
                contextMenuStrip1.Items.Add(item);
                contextMenuStrip1.Items[0].DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
        }

        private IEnumerable<int> list;
        private int index = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            string language = _langs[comboBox1.SelectedIndex];
            if (comboBox2.Text == "태그")
                list = Hitomi.CrawlFromTag(textBox1.Text, language);
            else if (comboBox2.Text == "시리즈")
                list = Hitomi.CrawlFromSeries(textBox1.Text, language);
            index = 0;

            foreach (int i in list.Skip(index).Take((int)numericUpDown1.Value))
            {
                Manga m = new Manga(i);
                ListViewItem item = new ListViewItem(m.Name);
                item.SubItems.Add(m.Number);
                item.Tag = m;
                listView1.Items.Add(item);
            }

            index += (int)numericUpDown1.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (int i in list.Skip(index).Take((int)numericUpDown1.Value))
            {
                Manga m = new Manga(i);
                ListViewItem item = new ListViewItem(m.Name);
                item.SubItems.Add(m.Number);
                item.Tag = m;
                listView1.Items.Add(item);
            }

            index += (int)numericUpDown1.Value;
        }
    }
}
