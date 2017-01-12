using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace hitomi
{
    public partial class Downloader : Form
    {
        public Downloader()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Manga m = new Manga(textBox1.Text);
                m.Downloaded += (i) => this.Text = $"{i}/{m.Length}";
                m.Download(Path.Combine("Download", m.Name));
                MessageBox.Show("끝");
            }
        }
    }
}
