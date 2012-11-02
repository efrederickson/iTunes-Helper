/*
 * User: elijah
 * Date: 10/22/2012
 * Time: 12:23 PM
 * Copyright 2012 LoDC
 */
using System;
using System.Drawing;
using System.IO;
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
            int changedAmount = 0;
            foreach (IITFileOrCDTrack t in MainForm.Library.Tracks)
            {
                c++;
                
                Invoke(new Action(delegate { label1.Text = "(" + c + "/" + max + ", " + changedAmount + " updated) "; }));
                
                if (t.Podcast == false && !string.IsNullOrEmpty(t.Location))
                {
                    Invoke(new Action(delegate
                                      {
                                          label1.Text += t.Name;
                                      }));
                    try
                    {
                        string what = "";
                        bool changed = false;
                        TagLib.File f = TagLib.File.Create(t.Location);
                        if (!string.IsNullOrEmpty(t.Album) && f.Tag.Album != t.Album)
                        {
                            f.Tag.Album = t.Album;
                            changed = true;
                            what += "Album ";
                        }
                        if (!string.IsNullOrEmpty(t.Artist)
                            && ((f.Tag.Artists.Length > 0 && f.Tag.Artists[0] != t.Artist)
                                || f.Tag.Artists.Length == 0))
                        {
                            f.Tag.Artists = new String[] { t.Artist };
                            f.Tag.Performers = new String[] { t.Artist };
                            changed = true;
                            what += "Artist ";
                        }
                        if (false &&
                            (!string.IsNullOrEmpty(t.AlbumArtist) || !string.IsNullOrEmpty(t.Artist)
                             && ((!string.IsNullOrEmpty(t.AlbumArtist) && f.Tag.AlbumArtists.Length > 0 && t.AlbumArtist != f.Tag.AlbumArtists[0]) || f.Tag.AlbumArtists.Length == 0)
                            ))
                        {
                            f.Tag.AlbumArtists = new String[] { string.IsNullOrEmpty(t.AlbumArtist) ? t.Artist : t.AlbumArtist };
                            changed = true;
                            what += "AlbumArtist ";
                        }
                        if (!string.IsNullOrEmpty(t.Genre)
                            && ((f.Tag.Genres.Length > 0 && t.Genre != f.Tag.Genres[0]) || f.Tag.Genres.Length == 0))
                        {
                            f.Tag.Genres = new String[] { t.Genre };
                            changed = true;
                            what += "Genre ";
                        }
                        if (!string.IsNullOrEmpty(t.Lyrics) && f.Tag.Lyrics != t.Lyrics)
                        {
                            f.Tag.Lyrics = t.Lyrics;
                            changed = true;
                            what += "Lyrics ";
                        }
                        if (f.Tag.Year < t.Year)
                        {
                            f.Tag.Year = (uint)t.Year;
                            changed = true;
                            what += "Year ";
                        }
                        
                        if (t.Location.StartsWith("W:\\Music\\Audio"))
                        {
                            string loc = t.Location.Substring("W:\\Music\\Audio".Length);
                            if (loc.StartsWith("\\"))
                                loc = loc.Substring(1);
                            string artist = loc.Substring(0, loc.IndexOf('\\'));
                            loc = loc.Substring(loc.IndexOf('\\') + 1); // strip the '\'
                            //MessageBox.Show(artist + ":::::" + loc);
                            string album = loc.Contains("\\") ? loc.Substring(0, loc.IndexOf('\\')) : "";
                            string title = album.Length > 0 ? Path.GetFileNameWithoutExtension(loc.Substring(loc.IndexOf('\\') + 1)) : "";
                            
                            
                            if (string.IsNullOrEmpty(artist) == false && string.IsNullOrWhiteSpace(artist) == false)
                            {
                                if (f.Tag.Artists.Length == 0)
                                {
                                    f.Tag.Artists = new string[] { artist };
                                    f.Tag.Performers = new string[] { artist };
                                    changed = true;
                                    what += "FromPath:Artist ";
                                }
                            }
                            
                            if (string.IsNullOrEmpty(album) == false && string.IsNullOrWhiteSpace(album) == false)
                            {
                                if (string.IsNullOrEmpty(f.Tag.Album) || string.IsNullOrWhiteSpace(f.Tag.Album))
                                    //f.Tag.Album != album)
                                {
                                    f.Tag.Album = album;
                                    changed = true;
                                    what += "FromPath:Album";
                                }
                            }
                            
                            if (title != "")
                            {
                                if (string.IsNullOrEmpty(f.Tag.Title))
                                {
                                    f.Tag.Title = title;
                                    changed = true;
                                    what += "FromPath:Title ";
                                }
                            }
                        }
                        
                        
                        string shortPath = Path.GetPathRoot(t.Location) + "...\\" + Path.GetFileName(t.Location);
                        if (changed)
                        {
                            changedAmount++;
                            Invoke(new Action(()=> { listBox1.Items.Add("Updated [" + t.Location + "] Was Changed: " + what); }));
                            //Invoke(new Action(()=> { listBox1.Items.Add("Updated [" + shortPath + "] Was Changed: " + what); }));
                            f.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(delegate { listBox1.Items.Add("Error: " + ex.Message); }));
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
