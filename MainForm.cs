/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 10/25/2011
 * Time: 11:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesHelper
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public static List<Device> ManagedDrives = new List<Device>();
        public static iTunesLib.iTunesApp iTunes = new iTunesLib.iTunesApp();
        public static IITPlaylist Library = iTunes.LibraryPlaylist;
        public static List<ScheduledSync> ScheduledSyncs = new List<ScheduledSync>();
        public static MainForm Instance;
        public static Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager Taskbar = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            Instance = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Taskbar.SetProgressValue(0, Library.Tracks.Count);
            Hashtable deleted = new Hashtable();
            Hashtable files = new Hashtable();
            int i = 0;
            foreach (IITFileOrCDTrack track in Library.Tracks)
            {
                Taskbar.SetProgressValue(++i, Library.Tracks.Count);
                if (files.ContainsKey(track.Location))
                {
                    track.Delete();
                    deleted[track] = track;
                }
                else
                {
                    files[track.Location] = track;
                }
            }
            string results = "";
            Taskbar.SetProgressValue(0, 1);
            foreach (IITFileOrCDTrack track in deleted)
                results += "Deleted track " + track.Name + " from " + track.Location + "\r\n";
            MessageBox.Show("Deleted " + deleted.Count + " track(s)\r\n" + results);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Taskbar.SetProgressValue(0, Library.Tracks.Count);
            List<IITFileOrCDTrack> deleted = new List<IITFileOrCDTrack>();
            int i = 0;
            foreach (IITFileOrCDTrack track in Library.Tracks)
            {
                Taskbar.SetProgressValue(++i, Library.Tracks.Count);
                if (track.Location == "" || (!System.IO.File.Exists(track.Location)))
                {
                    deleted.Add(track);
                    track.Delete();
                }
            }
            string results = "";
            Taskbar.SetProgressValue(0, 1);
            foreach (IITFileOrCDTrack track in deleted)
                results += "Deleted track " + track.Name + "\r\n";
            MessageBox.Show("Deleted " + deleted.Count + " track(s)\r\n" + results);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select folder to move library to: ";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                MovingForm MF = new MovingForm(fbd.SelectedPath);
                MF.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Taskbar.SetProgressValue(0, Library.Tracks.Count);
            int i = 0;
            List<IITFileOrCDTrack> deleted = new List<IITFileOrCDTrack>();
            foreach (IITFileOrCDTrack track in Library.Tracks)
            {
                Taskbar.SetProgressValue(++i, Library.Tracks.Count);
                if (track.Location == "" || (!System.IO.File.Exists(track.Location)))
                {
                    deleted.Add(track);
                }
            }
            string results = "";
            Taskbar.SetProgressValue(0, 1);
            foreach (IITFileOrCDTrack track in deleted)
                results += "Found track " + track.Name + "\r\n";
            MessageBox.Show("Found " + deleted.Count + " track(s)\r\n" + results);
        }

        private void AdvancedDuplicateScan(object sender, EventArgs e)
        {
            CheckingFilesForm CFF = new CheckingFilesForm();
            CFF.progressBar1.Value = 0;
            CFF.progressBar2.Maximum = Library.Tracks.Count;
            CFF.label1.Text = "Checking...";
            Hashtable deleted = new Hashtable();
            Hashtable files = new Hashtable();
            CFF.Show();
            foreach (IITFileOrCDTrack track in Library.Tracks)
            {
                CFF.label1.Text = "Checking " + track.Name + "...";
                if (string.IsNullOrEmpty(track.Location))
                    continue;
                if (files.ContainsKey(track.Location))
                {
                    track.Delete();
                    deleted[track] = track;
                }
                else
                {
                    CFF.progressBar1.Value = 0;
                    CFF.progressBar1.Maximum = files.Count;
                    foreach (IITFileOrCDTrack _track in files.Values)
                    {
                        CFF.progressBar1.Value++;
                        CFF.label1.Text = "Checking '" + track.Name + "'Against " + _track.Name;
                        try
                        {
                            if (_track.Name == track.Name)
                            {
                                if (_track.Duration == track.Duration)
                                {
                                    if (_track.Album == track.Album)
                                    {
                                        if (_track.Artist == track.Artist)
                                        {
                                            if (_track.Genre == track.Genre)
                                            {
                                                track.Delete();
                                                CFF.label1.Text = "Deleting " + track.Name;
                                            }
                                            else
                                                files[track.Location] = track;
                                        }
                                        else
                                            files[track.Location] = track;
                                    }
                                    else
                                        files[track.Location] = track;
                                }
                                else
                                    files[track.Location] = track;
                            }
                            else
                                files[track.Location] = track;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                CFF.progressBar2.Value++;
            }
            string results = "";
            foreach (IITFileOrCDTrack track in deleted)
                results += "Deleted track " + track.Name + " from " + track.Location + "\r\n";
            MessageBox.Show("Deleted " + deleted.Count + " track(s)\r\n" + results);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SelectDriveForm sdf = new SelectDriveForm();
            if (sdf.ShowDialog() == DialogResult.OK)
            {
                // check if a removable drive
                System.IO.DriveInfo di = sdf.SelectedDrive;
                if (di.DriveType != System.IO.DriveType.Removable)
                {
                    if (di.DriveType == System.IO.DriveType.Ram)
                    {
                        MessageBox.Show("You have selected a RAM drive. This is not allowed.");
                        return;
                    }
                    else if (di.DriveType == System.IO.DriveType.NoRootDirectory)
                    {
                        MessageBox.Show("The Selected drive doesn't have a root Directory!");
                        return;
                    }
                    DialogResult r = MessageBox.Show("Warning: The drive you have selected isn't a removable drive.\nContinue?", "OH NOES!", MessageBoxButtons.YesNo);
                    if (r == System.Windows.Forms.DialogResult.No)
                        return;
                }
                // generate Drive info
                IITPlaylist device = GetPlaylistNameForm.InputBox();
                Device newDevice = new Device(device, di);
                listBox1.Items.Add(newDevice.Name.ToUpper() + " (" + newDevice.iTunesPlaylist.Name + ")");
                ManagedDrives.Add(newDevice);
                //MessageBox.Show("To put songs on your device, drag them onto the playlist, \n" + "Then press 'sync'");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                listBox1.Items.Remove(listBox1.SelectedItem);
                ManagedDrives.RemoveAt(index);
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            // sync device
            SyncingForm SF = new SyncingForm();
            Device d = ManagedDrives[listBox1.SelectedIndex];
            SF.setMax(d.iTunesPlaylist.Tracks.Count);
            SF.SetValue(0);
            SF.SetText("Syncing...");
            SF.Show();
            int v = 0;
            foreach (IITFileOrCDTrack track in d.iTunesPlaylist.Tracks)
            {
                SF.SetText("Syncing " + track.Name + "...");
                // check if empty track
                if (track.Location == "")
                    continue;
                try
                {
                    // format output path
                    string outputPath = d.Drive.Name + "\\Music\\" + CleanName(string.IsNullOrEmpty(track.Artist) ? "Unknown Artist" : track.Artist) + "\\" + CleanName(string.IsNullOrEmpty(track.Album) ? "Unknown Album" : track.Album) + "\\" +
                        System.IO.Path.GetFileName(track.Location);
                    // write file to drive
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
                    if (System.IO.File.Exists(outputPath)) // check if it already exists
                        System.IO.File.Delete(outputPath); // if so, delete it
                    System.IO.File.Copy(track.Location, outputPath); // copy the file over
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    SF.SetValue(++v); // increase the progress bar
                }
            }
            SF.Close();
        }

        internal static string CleanName(string dirty)
        {
            foreach (char i in System.IO.Path.GetInvalidFileNameChars())
                dirty = dirty.Replace(i, '_');
            foreach (char i in System.IO.Path.GetInvalidPathChars())
                dirty = dirty.Replace(i, '_');
            if (dirty.EndsWith("."))
                dirty += "_";
            return dirty;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void setUpNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // redirect to Mount Media Drive
            button6_Click(sender, e);
        }

        private void exitoniTunesCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exitoniTunesCloseToolStripMenuItem.Checked)
                iTunes.OnQuittingEvent += new _IiTunesEvents_OnQuittingEventEventHandler(iTunes_OnQuittingEvent);
            else
                iTunes.OnQuittingEvent -= iTunes_OnQuittingEvent;
        }

        void iTunes_OnQuittingEvent()
        {
            // close this also.
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(10, "iTunes Helper", "Started!", ToolTipIcon.Info);
            
            LibraryLyricScanner.Start();
        }
        
        #region OVERRIDE WndProc
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MINIMIZE = 0xF020;
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() == SC_MINIMIZE)
                {
                    Hide();
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }
        
        void Button11_Click(object sender, EventArgs e)
        {
            new Forms.ConvertItemForm().ShowDialog();
        }
        
        void Button12_Click(object sender, EventArgs e)
        {
            if (scheduledSyncsListBox.SelectedIndex == -1)
                return;
            // start
            ScheduledSyncs[scheduledSyncsListBox.SelectedIndex].Start();
        }
        
        void Button13_Click(object sender, EventArgs e)
        {
            if (scheduledSyncsListBox.SelectedIndex == -1)
                return;
            // stop
            ScheduledSyncs[scheduledSyncsListBox.SelectedIndex].Stop();
        }
        
        void Button14_Click(object sender, EventArgs e)
        {
            if (scheduledSyncsListBox.SelectedIndex == -1)
                return;
            //remove
            ScheduledSyncs[scheduledSyncsListBox.SelectedIndex].Stop();
            ScheduledSyncs[scheduledSyncsListBox.SelectedIndex].Dispose();
            ScheduledSyncs.Remove(ScheduledSyncs[scheduledSyncsListBox.SelectedIndex]);
            listBox1.Items.Remove(listBox1.SelectedItem);
        }
        
        void Button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            Forms.SetupScheduledSyncForm f = new Forms.SetupScheduledSyncForm(ManagedDrives[listBox1.SelectedIndex]);
            f.ShowDialog();
            ScheduledSyncs.Add(f.result);
            scheduledSyncsListBox.Items.Add(ManagedDrives[listBox1.SelectedIndex].Name);
        }
        
        void Button15_Click(object sender, EventArgs e)
        {
            new Forms.LibraryBackupForm().Show();
            this.button15.Enabled = false;
        }
        
        bool hasTriedToCloseBefore = false;
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MWCCheckBox.Checked)
            {
                this.Hide();
                e.Cancel = true;
                if (!hasTriedToCloseBefore)
                {
                    notifyIcon1.ShowBalloonTip(1, "iTunes Helper", "Minimized!", ToolTipIcon.Info);
                    hasTriedToCloseBefore = true;
                }
            }
        }
        
        void ExitITunesHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        
        void Button9_Click(object sender, EventArgs e)
        {
            new Forms.OnTopMusicControllerForm().Show();
            button9.Enabled = false;
        }
        
        public void ShowBalloonTip(string msg, string title)
        {
            notifyIcon1.ShowBalloonTip(1000, title, msg, ToolTipIcon.None);
        }
        
        public bool GetLyrics
        {
            get
            {
                return checkBox1.Checked;
            }
        }
        
        public void SetMessage(string texT)
        {
            statusLabel.Text = texT;
        }
    }
}