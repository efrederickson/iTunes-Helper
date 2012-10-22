using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

namespace MediaPlayer.Lyrics
{
    /// <summary>
    /// Gets lyrics for songs
    /// </summary>
    public class LyricsFetchManager
    {
        public void Start()
        {
            foreach (iTunesLib.IITFileOrCDTrack t2 in this.t.Tracks)
            {
                iTunesLib.IITFileOrCDTrack t = t2;
                if (t == null)
                    continue;
                if (t.Location == "")
                    continue;
                
                if (t.Podcast)
                    continue;
                if (t.Location.ToLower().EndsWith(".m4") ||
                    t.Location.ToLower().EndsWith(".m4v"))
                    continue;
                    
                
                // skip ones that already have lyrics
                if (!string.IsNullOrEmpty(t.Lyrics))
                    continue;

                string result = string.Empty;
                ILyricsSource rSource = null;
                foreach (ILyricsSource source in this.Sources)
                {
                    if (source == null)
                        continue;
                    string s = source.GetLyrics(t);
                    //System.Windows.Forms.MessageBox.Show(source.Name + " :: " + s);
                    if (OnStatusChanged != null)
                        OnStatusChanged(null, new LyricEventArgs(source, s, !string.IsNullOrEmpty(s), t));
                    if (string.IsNullOrEmpty(s) == false)
                    {
                        result = s;
                        rSource = source;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(result) == false)
                {
                    t.Lyrics = result;
                    try
                    {
                        //t.Info.Save();
                        //MainWindow.Instance.StatusMessage = String.Format("Saved lyrics from {0}!", rSource.Name);
                    }
                    catch (Exception ex)
                    {
                        //MainWindow.Instance.StatusMessage = String.Format("Error saving lyrics: {0}", ex.Message);
                    }
                }
            }
        }

        public iTunesLib.IITPlaylist t;
        public LyricsFetchManager(iTunesLib.IITPlaylist t)
        {
            this.t = t;
        }
        public List<ILyricsSource> Sources = new List<ILyricsSource>();

        public void RegisterSource(ILyricsSource s)
        {
            Sources.Add(s);
        }

        public event EventHandler<LyricEventArgs> OnStatusChanged;

        public class LyricEventArgs : EventArgs
        {
            public ILyricsSource Source { get; private set; }
            public string Lyrics { get; private set; }
            public bool LyricsFound { get; private set; }
            public iTunesLib.IITFileOrCDTrack Track { get; private set; }

            public LyricEventArgs(ILyricsSource s, string lyrics, bool found, iTunesLib.IITFileOrCDTrack t)
            {
                this.Source = s;
                this.Lyrics = lyrics;
                this.Track = t;
                this.LyricsFound = found;
            }
        }
    }
}