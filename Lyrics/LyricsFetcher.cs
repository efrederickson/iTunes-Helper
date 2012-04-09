using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace iTunesHelper
{
    class LibraryLyricScanner
    {
        static public void Start()
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerAsync();
        }
        static int trackCount, currentTrack;

        static void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            SetMessage("Initializing lyric scanner...");
            while (true)
            {
                try
                {
                    trackCount = MainForm.iTunes.LibraryPlaylist.Tracks.Count;
                    currentTrack = 0;
                    foreach (iTunesLib.IITFileOrCDTrack track in MainForm.iTunes.LibraryPlaylist.Tracks)
                    {
                        currentTrack++;
                        if (track.Location == "")
                            continue;
                        if (IsValid(track.Location) == false)
                            continue;
                        
                        SetMessage("Scanning for " + track.Name);
                        
                        if (MainForm.Instance.GetLyrics == false)
                            continue;
                        if (string.IsNullOrEmpty(track.Lyrics.Trim()) == true)
                        {
                            LyricsFetchManager man = new LyricsFetchManager();
                            man.RegisterSource(new LyricsSourceLyrdb());           // LyricsFetcher
                            //man.RegisterSource(new Lyrics.LyricsSourceLyricsPlugin());    // BROKEN
                            man.RegisterSource(new LyricsSourceLyricsFly());       // LoDC
                            man.RegisterSource(new LyricsSourceAZLyrics());        // iTuner
                            man.RegisterSource(new LyricsSourceLyrics007());       // iTuner
                            man.RegisterSource(new LyricsSourceMP3Lyrics());       // iTuner
                            man.RegisterSource(new LyricsSourceAbsoluteLyrics());  // LoDC
                            man.Queue(track);
                            man.StatusEvent += new EventHandler<LyricsFetchStatusEventArgs>(man_StatusEvent);
                            man.Start();
                        }
                    }
                    
                }
                catch (Exception)
                {
                    
                }
                System.Threading.Thread.Sleep(50);
            }
        }
        static int lyricsFound = 0;
        
        static void SetMessage(string msg)
        {
            MainForm.Instance.Invoke(
                new EventHandler(
                    delegate
                    {
                        MainForm.Instance.SetMessage(string.Format("{0}/{1} tracks scanned, {2} lyrics found, {3}",
                                                                   currentTrack, trackCount, lyricsFound, msg
                                                                  ));
                        
                    } ));
        }
        
        static bool IsValid(string fn)
        {
            foreach (string ext in new string[] { ".m4a", ".mp3", ".wma", ".wav", ".aif", ".aiff", ".ogg"})
            {
                if (fn.ToLower().EndsWith(ext.ToLower()))
                    return true;
            }
            return false;
        }
        
        static void man_StatusEvent(object sender, LyricsFetchStatusEventArgs e)
        {
            string subText = "";
            switch (e.Status)
            {
                case iTunesHelper.LyricsFetchStatus.Undefined:
                    return;
                case iTunesHelper.LyricsFetchStatus.NotFound:
                    subText = "Lyrics not found!";
                    break;
                case iTunesHelper.LyricsFetchStatus.Waiting:
                    subText = "Waiting...";
                    break;
                case iTunesHelper.LyricsFetchStatus.Fetching:
                    subText = "Fetching lyrics...";
                    break;
                case iTunesHelper.LyricsFetchStatus.SourceDone:
                    subText = "Done searching source " + e.LyricsSource.Name + "!";
                    break;
                case iTunesHelper.LyricsFetchStatus.Done:
                    subText = string.Format("Done getting lyrics! Time taken: {0} seconds, found lyrics: {1}", e.ElapsedTime.ToString(), e.LyricsFound == true ? "yes" : "no");
                    if (e.LyricsFound)
                        lyricsFound++;
                    break;
                case iTunesHelper.LyricsFetchStatus.Cancelled:
                    subText = "Lyrics search cancelled!";
                    break;
                default:
                    break;
            }
            SetMessage(subText);

            //MainWindow.Instance.StatusMessage = subText;
        }

    }
    
    /// <summary>
    /// A LyricsSource represents a mechanism for finding the lyrics to a song.
    /// </summary>
    /// <remarks>
    /// Sources must be thread safe, since they will be used to fetch different songs at the same
    /// time in different threads.
    /// </remarks>
    public interface ILyricsSource
    {

        /// <summary>
        /// Gets the name of this source
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Fetch the lyrics for the given song
        /// </summary>
        /// <param name="s">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>
        string GetLyrics(iTunesLib.IITFileOrCDTrack s);
    }

    /// <summary>
    /// What is the status of the fetch operation?
    /// </summary>
    public enum LyricsFetchStatus
    {
        Undefined = 0,
        NotFound,
        Waiting,
        Fetching,
        SourceDone,
        Done,
        Cancelled
    }

    /// <summary>
    /// The information available in a FetchStatusEvent
    /// </summary>
    public class LyricsFetchStatusEventArgs : EventArgs
    {
        /// <summary>
        /// What is the fetch status of this song?
        /// </summary>
        public LyricsFetchStatus Status = LyricsFetchStatus.Undefined;
        
        /// <summary>
        /// What song is the event for?
        /// </summary>
        public iTunesLib.IITFileOrCDTrack Song;
        
        /// <summary>
        /// What lyrics source was involved?
        /// </summary>
        public ILyricsSource LyricsSource;

        /// <summary>
        /// Were the lyrics actually found?
        /// </summary>
        /// <remarks>
        /// This is only valid when Status == FetchStatus.SourceDone
        /// </remarks>
        public bool LyricsFound;
        
        /// <summary>
        /// How many milliseconds elapsed in searching the given source
        /// </summary>
        public int ElapsedTime;

        /// <summary>
        /// What were the lyrics
        /// </summary>
        /// <remarks>
        /// This is only valid when Status == FetchStatus.SourceDone
        /// </remarks>
        public string Lyrics;
    }

    /// <summary>
    /// A LyricsFetcher handles a single attempt to fetch the lyrics of a given
    /// song. It is designed to be run as a background task, using FetchSongLyrics
    /// as the thread entry point. Progress, success and failure are all reported
    /// through the StatusEvent event.
    /// </summary>
    public class LyricsFetcher
    {
        /// <summary>
        /// Get or set where this fetcher look to find its lyrics?
        /// </summary>
        public IList<ILyricsSource> Sources
        {
            get { return this.sources; }
            set { this.sources = value; }
        }
        
        private IList<ILyricsSource> sources = new List<ILyricsSource>();

        /// <summary>
        /// This the main thread entry point
        /// </summary>
        /// <param name="song"></param>
        public void FetchSongLyrics(object param)
        {
            iTunesLib.IITFileOrCDTrack song = (iTunesLib.IITFileOrCDTrack)param;

            this.StartFetch(song);
            string lyrics = this.DoFetch(song);
            this.FinishFetch(lyrics);
        }

        private void StartFetch(iTunesLib.IITFileOrCDTrack song)
        {
            this.args.Song = song;
        }

        private string DoFetch(iTunesLib.IITFileOrCDTrack song)
        {
            string lyrics = String.Empty;

            foreach (ILyricsSource source in this.Sources) {
                this.StartSource(source);

                int startTickCount = Environment.TickCount;
                lyrics = source.GetLyrics(song);
                this.FinishSource(source, lyrics, Environment.TickCount - startTickCount);

                if (lyrics != String.Empty)
                    return lyrics;
            }

            return String.Empty;
        }

        private void StartSource(ILyricsSource source)
        {
            this.args.Status = LyricsFetchStatus.Fetching;
            this.args.LyricsSource = source;
            this.OnStatusEvent(args);
        }

        private void FinishSource(ILyricsSource source, string lyrics, int elapsedTime)
        {
            this.args.Status = LyricsFetchStatus.SourceDone;
            this.args.LyricsSource = source;
            this.args.Lyrics = lyrics;
            this.args.ElapsedTime = elapsedTime;
            this.OnStatusEvent(args);
        }

        private void FinishFetch(string lyrics)
        {
            this.args.Status = LyricsFetchStatus.Done;
            this.args.Lyrics = lyrics;
            this.OnStatusEvent(args);
        }

        #region Events

        /// <summary>
        /// Signal that the fetch status may have changed
        /// </summary>
        public event EventHandler<LyricsFetchStatusEventArgs> StatusEvent;

        protected virtual void OnStatusEvent(LyricsFetchStatusEventArgs args)
        {
            if (this.StatusEvent != null)
                this.StatusEvent(this, args);
        }

        #endregion

        #region Private variables

        private LyricsFetchStatusEventArgs args = new LyricsFetchStatusEventArgs();

        #endregion
    }
}
