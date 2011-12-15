using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesHelper
{
    public partial class MovingForm : Form
    {
        private string outDir;

        public MovingForm(string outDir)
        {
            InitializeComponent();
            this.outDir = outDir;
        }

        private void MovingForm_Load(object sender, EventArgs e)
        {
            progressBar2.Maximum = MainForm.iTunes.LibraryPlaylist.Tracks.Count;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (IITFileOrCDTrack track in MainForm.Library.Tracks)
            {
                backgroundWorker1.ReportProgress(-1, "Processing File " + track.Name + " (" + track.Location + ")..."); 
                // only music files
                if (string.IsNullOrEmpty(track.Location))
                {
                    backgroundWorker1.ReportProgress(-2);
                    continue;
                }
                if (track.Location.EndsWith(".mp3") || track.Location.EndsWith(".m4a") || track.Location.EndsWith(".wma") || track.Location.EndsWith(".wav"))
                {
                    string newDir = outDir + "\\" + CleanName(string.IsNullOrEmpty(track.Artist) ? "Unknown Artist" : track.Artist) + "\\" + CleanName(string.IsNullOrEmpty(track.Album) ? "Unknown Album" : track.Album) + "\\";
                    string newFileName = CleanName(track.Name) + System.IO.Path.GetExtension(track.Location);
                    string fullName = newDir + newFileName;
                    string old = track.Location;
                    backgroundWorker1.ReportProgress(-1, "Copying File " + track.Location);
                    // copy new file
                    try
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullName));
                        if (!System.IO.File.Exists(fullName))
                        { 
                            System.IO.File.Move(old, fullName);
                        // apply changes
                        track.Delete();
                        MainForm.iTunes.LibraryPlaylist.AddFile(fullName);
                        }
                        //MessageBox.Show("Moved  " + old + " to " + fullName);
                    }
                    catch (Exception ex)
                    {
                        backgroundWorker1.ReportProgress(-1, ex.Message);
                        backgroundWorker1.ReportProgress(-3, "Error: " + ex.Message + " Filename: " + fullName);
                    }
                }
                // update total progress bar
                backgroundWorker1.ReportProgress(-2);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
                label1.Text = (string)e.UserState;
            else if (e.ProgressPercentage == -2)
                progressBar2.Value++;
            else if (e.ProgressPercentage == -3)
                listBox1.Items.Add((string)e.UserState);
            else
            {
                if (e.ProgressPercentage >= progressBar1.Maximum)
                    progressBar1.Maximum = e.ProgressPercentage + 1;
                progressBar1.Value = e.ProgressPercentage;
                label1.Text = (string)e.UserState;
            }
        }

        private string CleanName(string dirty)
        {
            foreach (char i in System.IO.Path.GetInvalidFileNameChars())
                dirty = dirty.Replace(i, '_');
            foreach (char i in System.IO.Path.GetInvalidPathChars())
                dirty = dirty.Replace(i, '_');
            if (dirty.EndsWith("."))
                dirty += "_";
            return dirty;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done moving Library!");
        }
    }
}