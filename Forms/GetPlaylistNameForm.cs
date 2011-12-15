using System;
using iTunesLib;
using System.Windows.Forms;

namespace iTunesHelper
{
    public partial class GetPlaylistNameForm : Form
    {
        private IITPlaylist Result;
        private GetPlaylistNameForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.iTunes.CreatePlaylistInSource(textBox1.Text, MainForm.iTunes.Sources.ItemByName["Library"]);
            listBox1.Items.Add("Library->" + textBox1.Text);
        }

        public static IITPlaylist InputBox()
        {
            GetPlaylistNameForm gdnf = new GetPlaylistNameForm();
            gdnf.ShowDialog();
            return gdnf.Result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // use the selected Playlist
            if (listBox1.SelectedIndex == -1)
                return;
            string s = (string)listBox1.SelectedItem;
            string parent = s.Split(new string[] { "->" }, StringSplitOptions.None)[0];
            string child = s.Split(new string[] { "->" }, StringSplitOptions.None)[1];
            IITSource source = MainForm.iTunes.Sources.ItemByName[parent];
            IITPlaylist p = source.Playlists.ItemByName[child];
            Result = p;
            this.Close();
        }

        private void GetDeviceNameForm_Load(object sender, EventArgs e)
        {
            foreach (IITSource s in MainForm.iTunes.Sources)
            {
                foreach (IITPlaylist p in s.Playlists)
                {
                    listBox1.Items.Add(s.Name + "->" + p.Name);
                }
            }
        }
    }
}
