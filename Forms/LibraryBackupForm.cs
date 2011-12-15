/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 11/11/2011
 * Time: 8:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using Ionic.Zlib;
using iTunesLib;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// backups up library
    /// </summary>
    public partial class LibraryBackupForm : Form
    {
        public static Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager Taskbar = TaskbarManager.Instance;
        // iTunes
        public static iTunesLib.iTunesAppClass iTunes = new iTunesLib.iTunesAppClass();
        public static string Password = string.Empty;
        // Ionic.Zip
        EncryptionAlgorithm MahEncType;
        private bool _saveCanceled;
        private long _totalBytesAfterCompress;
        private long _totalBytesBeforeCompress;
        private int _nFilesCompleted;
        private int _progress2MaxFactor;
        private int _entriesToZip;
        // Delegates for invocation of UI from other threads
        private delegate void SaveEntryProgress(SaveProgressEventArgs e);
        private delegate void ButtonClick(object sender, EventArgs e);
        BackgroundWorker bgw = new BackgroundWorker();
        
        public LibraryBackupForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            outputDirTextBox.Text = "C:\\Album Zips";
        }
        
        void BtnDirBrowse_Click(object sender, EventArgs e)
        {
            string folderName = this.outputDirTextBox.Text;
            SaveFileDialog dlg1 = new SaveFileDialog();
            dlg1.Filter = "Zip archives (*.zip)|*.zip";
            if (dlg1.ShowDialog() == DialogResult.OK)
            {
                //Me._folderName = dlg1.get_SelectedPath
                this.outputDirTextBox.Text = dlg1.FileName;
            }
        }
        
        void BtnZipUp_Click(object sender, EventArgs e)
        {
            Taskbar.SetProgressState(TaskbarProgressBarState.Normal);
            if (MahEncType != EncryptionAlgorithm.None)
            {
                GetEncryptionCodeForm R = new GetEncryptionCodeForm("iTunes album zips");
                R.ShowDialog();
                Password = R.Password;
            }
            
            Taskbar.SetProgressValue(0, 1);
            bgw.DoWork += delegate(object sender2, DoWorkEventArgs e2) {
                try
                {
                    this.KickoffZipup(outputDirTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };
            bgw.RunWorkerAsync();
        }
        
        private void KickoffZipup(string outfile)
        {
            if (File.Exists(outfile))
            {
                if ((MessageBox.Show(string.Format("The file you have specified ({0}) already exists.  Do you want to overwrite this file?", outfile), "Zipper - Zip Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)) {
                    return;
                }
                else
                {
                    File.Delete(outfile);
                }
            }
            this._saveCanceled = false;
            this._nFilesCompleted = 0;
            this._totalBytesAfterCompress = 0;
            this._totalBytesBeforeCompress = 0;
            this.btnZipUp.Invoke(new ButtonClick(delegate (object sender, EventArgs e) {
                                                     btnZipUp.Enabled = false; this.btnZipUp.Text = "Zipping...";
                                                     this.btnCancel.Enabled = true;
                                                     this.lblStatus.Text = "Loading...";
                                                 }), new object[] {null, EventArgs.Empty});
            //this.btnZipUp.Enabled = false;
            //this.btnZipUp.Text = "Zipping...";
            //this.btnCancel.Enabled = true;
            //this.lblStatus.Text = "Loading...";
           Classes.WorkerOptions options = new Classes.WorkerOptions();
            options.ZipName = outfile;

            //_backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            //_backgroundWorker1.WorkerSupportsCancellation = false;
            //_backgroundWorker1.WorkerReportsProgress = false;
            //this._backgroundWorker1.DoWork += new DoWorkEventHandler(this.DoSave);
            //_backgroundWorker1.RunWorkerAsync(new object[] {options, outfile});
            DoSave(null, new DoWorkEventArgs(options));
        }

        private void DoSave(object sender, DoWorkEventArgs e)
        {
           Classes.WorkerOptions options = e.Argument as Classes.WorkerOptions;
            try {
                using (ZipFile zip1 = new ZipFile()) {
                    if (MahEncType != EncryptionAlgorithm.None)
                    {
                        zip1.Encryption = MahEncType;
                        zip1.Password = Password;
                    }
                    Taskbar.SetProgressValue(0, iTunes.LibraryPlaylist.Tracks.Count);
                    int i = 0;
                    this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                             lblStatus.Text = "Processing files...";
                                                             ProgressBar1.Value = 0;
                                                             ProgressBar1.Maximum = iTunes.LibraryPlaylist.Tracks.Count;
                                                         }), new object[] {null, EventArgs.Empty});
                    #region ' Adding files to Archive '
                    if (fullBackupRadioButton.Checked)
                    {
                        foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                        {
                            if (this._saveCanceled)
                                break;
                            this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                     ProgressBar1.Value ++;
                                                                 }), new object[] {null, EventArgs.Empty});
                            
                            Taskbar.SetProgressValue(++i, iTunes.LibraryPlaylist.Tracks.Count);
                            if (!string.IsNullOrEmpty(track.Location))
                            {
                                this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + track.Location;
                                                                     }), new object[] {null, EventArgs.Empty});
                                try {
                                    zip1.AddFile(track.Location, (string.IsNullOrEmpty(track.Artist) ? "Unknown Artist" : Clean(track.Artist) )+ "\\" + (string.IsNullOrEmpty(track.Album) ? "Unknown Album" : Clean(track.Album)) + "\\");
                                } catch (Exception ex) {
                                    this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                             errorListBox.Items.Add("Error adding a file: " + track.Location + " " +  ex.Message);
                                                                         }), new object[] {null, EventArgs.Empty});
                                }
                            }
                            else
                            {
                                this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + "found dead track!";
                                                                     }), new object[] {null, EventArgs.Empty});
                            }
                        }
                    }
                    else
                    {
                        foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                        {
                            if (this._saveCanceled)
                                break;
                            this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                     ProgressBar1.Value ++;
                                                                 }), new object[] {null, EventArgs.Empty});
                            
                            Taskbar.SetProgressValue(++i, iTunes.LibraryPlaylist.Tracks.Count);
                            if (!string.IsNullOrEmpty(track.Location))
                            {
                                this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + track.Location;
                                                                     }), new object[] {null, EventArgs.Empty});
                                try {
                                    if ((!track.Podcast) && (track.Location.EndsWith(".wav") || track.Location.EndsWith(".m4a") || track.Location.EndsWith(".mp3")))
                                    {
                                        this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + track.Location;
                                                                     }), new object[] {null, EventArgs.Empty});
                                        zip1.AddFile(track.Location, (string.IsNullOrEmpty(track.Artist) ? "Unknown Artist" : Clean(track.Artist) )+ "\\" + (string.IsNullOrEmpty(track.Album) ? "Unknown Album" : Clean(track.Album)) + "\\");
                                    }
                                    else
                                    {
                                        this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + "skipping non-music file!";
                                                                     }), new object[] {null, EventArgs.Empty});
                                    }
                                } catch (Exception ex) {
                                    this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                             errorListBox.Items.Add("Error adding a file: " + track.Location + " " +  ex.Message);
                                                                         }), new object[] {null, EventArgs.Empty});
                                }
                            }
                            else
                            {
                                this.btnZipUp.Invoke(new ButtonClick(delegate (object sender2, EventArgs e2) {
                                                                         lblStatus.Text = "Processing files... " + "found dead track!";
                                                                     }), new object[] {null, EventArgs.Empty});
                            }
                        }
                    }
                    #endregion
                    Taskbar.SetProgressValue(0, zip1.EntryFileNames.Count);
                    this._entriesToZip = zip1.EntryFileNames.Count;
                    this.SetProgressBars();
                    zip1.CompressionLevel = CompressionLevel.BestCompression;
                    zip1.SaveProgress += new EventHandler<SaveProgressEventArgs>(this.zip1_SaveProgress);
                    zip1.Save(options.ZipName);
                }
            }
            catch (Exception exc1)
            {
                MessageBox.Show(string.Format("Error: {0}", exc1.Message));
                this.BtnCancel_Click(null, null);
            }
        }

        private void zip1_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (this._saveCanceled) {
                e.Cancel = true;
                return;
            }

            switch (e.EventType) {
                case ZipProgressEventType.Saving_AfterWriteEntry:
                    this.StepArchiveProgress(e);
                    Taskbar.SetProgressValue(e.EntriesSaved, e.EntriesTotal);
                    break; // TODO: might not be correct. Was : Exit Select
                case ZipProgressEventType.Saving_Completed:
                    this.SaveCompleted();
                    break; // TODO: might not be correct. Was : Exit Select
                case ZipProgressEventType.Saving_EntryBytesRead:
                    this.StepEntryProgress(e);
                    break; // TODO: might not be correct. Was : Exit Select
            }
        }

        private void StepArchiveProgress(SaveProgressEventArgs e)
        {
            if (this.ProgressBar1.InvokeRequired) {
                this.ProgressBar1.Invoke(new SaveEntryProgress(this.StepArchiveProgress), new object[] { e });
            } else if (!this._saveCanceled) {
                this._nFilesCompleted += 1;
                this.ProgressBar1.PerformStep();
                this._totalBytesAfterCompress = (this._totalBytesAfterCompress + e.CurrentEntry.CompressedSize);
                this._totalBytesBeforeCompress = (this._totalBytesBeforeCompress + e.CurrentEntry.UncompressedSize);
                // progressBar2 is the one dealing with the item being added to the archive
                // if we got this event, then the add of that item (or file) is complete, so we
                // update the progressBar2 appropriately.
                this.ProgressBar2.Value = this.ProgressBar2.Maximum = 1;
                base.Update();
            }
        }

        private void SaveCompleted()
        {
            if (this.lblStatus.InvokeRequired) {
                this.lblStatus.Invoke(new MethodInvoker(SaveCompleted));
                //Me.lblStatus.Invoke(New MethodInvoker(Me, DirectCast(Me.SaveCompleted, IntPtr)))
            } else {
                this.lblStatus.Text = string.Format("Done, Compressed {0} files, {1:N0}% of original", this._nFilesCompleted, ((100 * this._totalBytesAfterCompress) / Convert.ToDouble(this._totalBytesBeforeCompress)));
                this.ResetState();
            }
        }

        private void StepEntryProgress(SaveProgressEventArgs e)
        {
            if (this.ProgressBar2.InvokeRequired) {
                this.ProgressBar2.Invoke(new SaveEntryProgress(this.StepEntryProgress), new object[] { e });
            } else if (!this._saveCanceled) {
                if ((this.ProgressBar2.Maximum == 1)) {
                    long entryMax = e.TotalBytesToTransfer;
                    long absoluteMax = 0x7fffffff;
                    this._progress2MaxFactor = 0;
                    while ((entryMax > absoluteMax)) {
                        entryMax = (entryMax / 2);
                        this._progress2MaxFactor += 1;
                    }
                    if ((Convert.ToInt32(entryMax) < 0)) {
                        entryMax = (entryMax * -1);
                    }
                    this.ProgressBar2.Maximum = Convert.ToInt32(entryMax);
                }
                int xferred = Convert.ToInt32((e.BytesTransferred >> this._progress2MaxFactor));
                this.ProgressBar2.Value = ((xferred >= this.ProgressBar2.Maximum) ? this.ProgressBar2.Maximum : xferred);
                base.Update();
                this.lblStatus.Text = string.Format("{0} of {1} files...({2})", (this._nFilesCompleted + 1), this._entriesToZip, e.CurrentEntry.FileName);
            }
        }

        private void ResetState()
        {
            this.btnCancel.Enabled = false;
            this.btnZipUp.Enabled = true;
            this.btnZipUp.Text = "Zip";
            this.ProgressBar1.Value = 0;
            this.ProgressBar2.Value = 0;
            this.Cursor = Cursors.Default;
            Taskbar.SetProgressValue(0, 1);
        }

        private void SetProgressBars()
        {
            if (this.ProgressBar1.InvokeRequired) {
                //Me.ProgressBar1.Invoke(New MethodInvoker(Me, DirectCast(Me.SetProgressBars, IntPtr)))
                this.ProgressBar1.Invoke(new MethodInvoker(SetProgressBars));
            } else {
                this.ProgressBar1.Value = 0;
                this.ProgressBar1.Maximum = this._entriesToZip;
                this.ProgressBar1.Minimum = 0;
                this.ProgressBar1.Step = 1;
                this.ProgressBar2.Value = 0;
                this.ProgressBar2.Minimum = 0;
                this.ProgressBar2.Maximum = 1;
                this.ProgressBar2.Step = 2;
            }
        }
        
        void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.lblStatus.InvokeRequired) {
                this.lblStatus.Invoke(new ButtonClick(this.BtnCancel_Click), new object[] {
                                          sender,
                                          e
                                      });
            } else {
                this._saveCanceled = true;
                this.lblStatus.Text = "Canceled...";
                this.ResetState();
            }
        }
        
        void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked) {
                MahEncType = EncryptionAlgorithm.None;
            } else if (RadioButton2.Checked) {
                MahEncType = EncryptionAlgorithm.PkzipWeak;
            } else if (RadioButton3.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes128;
            } else if (RadioButton4.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes256;
            } else {
                MessageBox.Show("Error: Invalid Encryption Type!");
            }
        }
        
        void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked) {
                MahEncType = EncryptionAlgorithm.None;
            } else if (RadioButton2.Checked) {
                MahEncType = EncryptionAlgorithm.PkzipWeak;
            } else if (RadioButton3.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes128;
            } else if (RadioButton4.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes256;
            } else {
                MessageBox.Show("Error: Invalid Encryption Type!");
            }
        }
        
        void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked) {
                MahEncType = EncryptionAlgorithm.None;
            } else if (RadioButton2.Checked) {
                MahEncType = EncryptionAlgorithm.PkzipWeak;
            } else if (RadioButton3.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes128;
            } else if (RadioButton4.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes256;
            } else {
                MessageBox.Show("Error: Invalid Encryption Type!");
            }
        }
        
        void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked) {
                MahEncType = EncryptionAlgorithm.None;
            } else if (RadioButton2.Checked) {
                MahEncType = EncryptionAlgorithm.PkzipWeak;
            } else if (RadioButton3.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes128;
            } else if (RadioButton4.Checked) {
                MahEncType = EncryptionAlgorithm.WinZipAes256;
            } else {
                MessageBox.Show("Error: Invalid Encryption Type!");
            }
        }
        
        public string Clean(string d)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                d = d.Replace(c, '_');
            foreach (char c in System.IO.Path.GetInvalidPathChars())
                d = d.Replace(c, '_');
            if (d.EndsWith("."))
                d += " _";
            return d;
        }
        
        void MainForm_Load(object sender, EventArgs e)
        {
            outputDirTextBox.Text = "C:\\Elijah - Libraries\\BACKUPS\\iTunes.zip";
        }
        
        void LibraryBackupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bgw.Dispose();
            MainForm.Instance.button15.Enabled = true;
        }
    }
}
