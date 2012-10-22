/*
 * User: elijah
 * Date: 10/22/2012
 * Time: 12:23 PM
 * Copyright 2012 LoDC
 */
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// Description of ApplyLibraryToFiles.
    /// </summary>
    public partial class ApplyLibraryToFiles : Form
    {
        public ApplyLibraryToFiles()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            label1.Text = "";
            run();
            this.Closing += delegate { t.Abort(); };
        }
        Thread t = null;
        void run()
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 0;
            progressBar1.Maximum = MainForm.Library.Tracks.Count;
            label1.Text = "";
            t = new Thread(new ThreadStart(_run));
            t.Start();
        }
        
        void _run()
        {
            int c = 0;
            int max = progressBar1.Maximum;
            foreach (IITFileOrCDTrack t in MainForm.Library.Tracks)
            {
                c++;
                
                Invoke(new Action(delegate { label1.Text = "(" + c + "/" + max + ") "; }));
                
                if (t.Podcast == false && !string.IsNullOrEmpty(t.Location))
                {
                    Invoke(new Action(delegate
                                      {
                                          label1.Text += t.Name;
                                      }));
                    try
                    {
                        TagLib.File f = TagLib.File.Create(t.Location);
                        if (!string.IsNullOrEmpty(t.Album))
                            f.Tag.Album = t.Album;
                        if (!string.IsNullOrEmpty(t.Artist))
                        {
                            f.Tag.Artists = new String[] { t.Artist };
                            f.Tag.Performers = new String[] { t.Artist };
                        }
                        if (!string.IsNullOrEmpty(t.AlbumArtist) || !string.IsNullOrEmpty(t.Artist))
                            f.Tag.AlbumArtists = new String[] { string.IsNullOrEmpty(t.AlbumArtist) ? t.Artist : t.AlbumArtist };
                        if (!string.IsNullOrEmpty(t.Genre))
                            f.Tag.Genres = new String[] { t.Genre };
                        if (!string.IsNullOrEmpty(t.Lyrics))
                            f.Tag.Lyrics = t.Lyrics;
                        if (f.Tag.Year < t.Year)
                            f.Tag.Year = (uint)t.Year;
                        
                        f.Save();
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(delegate { listBox1.Items.Add(ex.Message); }));
                    }
                }
                else
                    Invoke(new Action(delegate { label1.Text += "..."; }));
                
                Invoke(new Action(delegate
                                  {
                                      progressBar1.Value++;
                                  }));
            }
        }
    }
}
