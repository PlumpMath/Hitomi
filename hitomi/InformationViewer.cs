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
    public partial class InformationViewer : Form
    {
        public InformationViewer()
        {
            InitializeComponent();
            AddListView("Name");
            AddListView("Artist");
            AddListView("Language");
            AddListView("Series");
            AddListView("Tags");
        }

        private void AddListView(string val)
        {
            ListViewItem i = new ListViewItem(val);
            i.SubItems.Add("");
            listView1.Items.Add(i);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Manga m = new Manga(Convert.ToInt32(textBox1.Text));
                listView1.Items[0].SubItems[1].Text = m.Name;
                listView1.Items[1].SubItems[1].Text = m.Artist;
                listView1.Items[2].SubItems[1].Text = m.Language;
                listView1.Items[3].SubItems[1].Text = m.Series;
                listView1.Items[4].SubItems[1].Text = string.Join(", ", m.Tags.Select(t => t.Original));
                pictureBox1.Image = m.GetImages().First();
            }
        }
    }
}
