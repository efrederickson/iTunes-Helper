using System;
using System.IO;
using System.Windows.Forms;

namespace iTunesHelper
{
    public partial class SelectDriveForm : Form
    {
        static string[] Letters = { "a", "b", "c", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        System.Collections.Generic.List<DriveInfo> Drives = new System.Collections.Generic.List<DriveInfo>();
        public DriveInfo SelectedDrive;
        public SelectDriveForm()
        {
            InitializeComponent();
            foreach (string letter in Letters)
            {
                try
                {
                    // attempt to connect to drive
                    System.IO.DriveInfo di = new System.IO.DriveInfo(letter + ":\\");
                    // check if its ready
                    if (di.IsReady)
                        Drives.Add(di);
                }
                catch (Exception ex)
                {
                    // there has to be a better way...
                }
            }

            // initialize drives
            foreach (DriveInfo d in Drives)
                drivesListBox.Items.Add(d);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (drivesListBox.SelectedIndex != -1)
            {
                DialogResult = DialogResult.OK;
                SelectedDrive = (DriveInfo)drivesListBox.SelectedItem;
                this.Close();
            }
            else
                MessageBox.Show("Please select a drive!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
