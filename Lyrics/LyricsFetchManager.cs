// 
// Taken from LyricsFetcher: http://lyricsfetcher.sourceforge.net/
// 


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

namespace iTunesHelper
{
    /// <summary>
    /// A LyricsFetchManager manages the process of fetching lyrics for several songs.
    /// The fetching is handled asynchronously.
    /// </summary>
    public class LyricsFetchManager
    {
        /// <summary>
        /// How many songs are currently fetching their lyrics?
        /// </summary>
        public int CountFetching {
            get {
                return this.fetchingSongs.Count;
            }
        }

        /// <summary>
        /// How many songs are waiting to start fetching their lyrics?
        /// </summary>
        public int CountWaiting {
            get {
                return this.waitingSongs.Count;
            }
        }

        /// <summary>
        /// Return true if any lyrics are currently being fetched or are waiting to be fetched.
        /// </summary>
        public bool IsFetching {
            get {
                return this.CountWaiting > 0 || this.CountFetching > 0;
            }
        }

        /// <summary>
        /// How many simultaneous threads will be used for fetching lyrics?
        /// </summary>
        public int MaxFetchingThreads {
            get { return this.maxFetching; }
            set { this.maxFetching = Math.Max(1, value); }
        }
        private int maxFetching = 5;

        /// <summary>
        /// Pause the fetching of lyrics. This does not stop existing threads --
        /// it simply prevents new threads from being launched
        /// </summary>
        public bool Paused {
            get { return this.paused; }
            set {
                this.paused = value;
                if (!this.paused)
                    this.PossibleStartNewThreads();
            }
        }
        private bool paused = true;

        #region Commands

        /// <summary>
        /// Start the process of fetching lyrics
        /// </summary>
        public void Start() {
            this.Paused = false;
        }

        /// <summary>
        /// Add the given collection of songs to those whose lyrics are being fetched
        /// </summary>
        /// <param name="songs"></param>
        public void Queue(IEnumerable<iTunesLib.IITFileOrCDTrack> songs) {
            foreach (iTunesLib.IITFileOrCDTrack song in songs) {
                // If the song is already being processed, ignore it
                if (!this.songStatusMap.ContainsKey(song)) {
                    this.waitingSongs.Add(song);
                    this.QueueInternal(song);
                }
            }
            this.PossibleStartNewThreads();
        }

        /// <summary>
        /// Add the song to the list of songs whose lyrics are being fetched
        /// </summary>
        /// <param name="song"></param>
        public void Queue(iTunesLib.IITFileOrCDTrack song) {
            this.Queue(new iTunesLib.IITFileOrCDTrack[] { song });
        }

        /// <summary>
        /// After new songs have been added, or old fetches have completed,
        /// possibly start a number of new threads up to our limit of concurrent
        /// fetches.
        /// </summary>
        protected void PossibleStartNewThreads() {
            if (this.Paused)
                return;

            // Prevent race conditions
            lock (this.thisLock) {
                while (this.CountWaiting > 0 && this.CountFetching < this.maxFetching) {
                    this.StartNewThread();
                }
            }
        }
        private Object thisLock = new Object();

        /// <summary>
        /// Start one new process for the first waiting song, removing
        /// that song from the wait queue
        /// </summary>
        protected void StartNewThread() {
            iTunesLib.IITFileOrCDTrack song = this.waitingSongs[0];
            this.waitingSongs.RemoveAt(0);
            this.fetchingSongs.Add(song);
            this.StartInternal(song);
        }

        /// <summary>
        /// Cancel the fetching of lyrics of the given song
        /// </summary>
        /// <param name="s"></param>
        public void Cancel(iTunesLib.IITFileOrCDTrack song) {
            this.CancelOne(song);
            this.PossibleStartNewThreads();
        }

        /// <summary>
        /// Cancel all fetches
        /// </summary>
        public void CancelAll() {
            foreach (iTunesLib.IITFileOrCDTrack song in this.waitingSongs.ToArray())
                this.CancelOne(song);
            foreach (iTunesLib.IITFileOrCDTrack song in this.fetchingSongs.ToArray())
                this.CancelOne(song);
        }

        /// <summary>
        /// Cancel the work on the given song
        /// </summary>
        /// <param name="song">The song to be cancelled</param>
        protected void CancelOne(iTunesLib.IITFileOrCDTrack song) {
            this.CancelInternal(song);
            this.waitingSongs.Remove(song);
            this.fetchingSongs.Remove(song);
            this.songStatusMap.Remove(song);
        }

        /// <summary>
        /// The given song has finished being worked on. Cleanup its 
        /// resources and start new tasks
        /// </summary>
        /// <param name="song"></param>
        protected void CleanupOne(iTunesLib.IITFileOrCDTrack song)
        {
            this.fetchingSongs.Remove(song);
            this.songStatusMap.Remove(song);
            this.PossibleStartNewThreads();
        }

        /// <summary>
        /// Wait until all lyrics have been fetched
        /// </summary>
        public void WaitUntilFinished() {
            while (this.IsFetching) {
                //System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }
        #endregion

        protected List<iTunesLib.IITFileOrCDTrack> waitingSongs = new List<iTunesLib.IITFileOrCDTrack>();
        protected List<iTunesLib.IITFileOrCDTrack> fetchingSongs = new List<iTunesLib.IITFileOrCDTrack>();
        protected Hashtable songStatusMap = new Hashtable();

        #region Public Attributes

        /// <summary>
        /// Once lyrics have been fetched, should they be automatically written into
        /// the      object?
        /// </summary>
        public bool AutoUpdateLyrics { get; set; }

        /// <summary>
        /// Where will fetcher used by this manager look to find their lyrics?
        /// </summary>
        /// <remarks>Use RegisterSource() to add a new source to the manager</remarks>
        public IList<ILyricsSource> Sources {
            get { return sources; }
        }
        private List<ILyricsSource> sources = new List<ILyricsSource>();

        #endregion

        #region Enquiries

        /// <summary>
        /// What is the status of the fetching of lyrics for the given song?
        /// </summary>
        /// <param name="s"></param>
        /// <returns>This can only return Waiting, Fetching or NotFound.</returns>
        public LyricsFetchStatus GetStatus(iTunesLib.IITFileOrCDTrack song)
        {
            if (this.songStatusMap.ContainsKey(song))
                return this.GetFetchRequestData(song).Status;
            else
                return LyricsFetchStatus.NotFound;
        }
        
        /// <summary>
        /// Return a textual description of the status of the fetch request
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public string GetStatusString(iTunesLib.IITFileOrCDTrack song)
        {
            LyricsFetchStatus status = this.GetStatus(song);
            switch (status) {
                case LyricsFetchStatus.NotFound:
                    return "Not found";
                case LyricsFetchStatus.Fetching:
                    FetchRequestData data = this.GetFetchRequestData(song);
                    if (data != null) {
                        ILyricsSource source = data.Source;
                        if (source != null)
                            return String.Format("Trying {0}...", source.Name);
                    }
                    return "Trying ...";
                default:
                    return status.ToString();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Register the given source so it is used by all subsequent fetches
        /// </summary>
        /// <param name="source">A source for lyrics</param>
        public void RegisterSource(ILyricsSource source)
        {
            this.Sources.Add(source);
        }

        #endregion

        #region Implementation
        
        /// <summary>
        /// Remove the given song from those being operated on.
        /// </summary>
        /// <param name="song">The song to be removed</param>
        public void CancelInternal(iTunesLib.IITFileOrCDTrack song)
        {
            LyricsFetchStatusEventArgs args = new LyricsFetchStatusEventArgs();
            args.Song = song;
            args.Status = LyricsFetchStatus.Cancelled;
            this.OnStatusEvent(args);
        }
        
        private FetchRequestData GetFetchRequestData(iTunesLib.IITFileOrCDTrack song) {
            return ((FetchRequestData)this.songStatusMap[song]);
        }

        /// <summary>
        /// Add the song to the list of songs whose lyrics are being fetched
        /// </summary>
        /// <param name="song"></param>
        protected void QueueInternal(iTunesLib.IITFileOrCDTrack song)
        {
            this.songStatusMap[song] = new FetchRequestData();

            LyricsFetchStatusEventArgs args = new LyricsFetchStatusEventArgs();
            args.Song = song;
            args.Status = LyricsFetchStatus.Waiting;
            this.OnStatusEvent(args);
        }
        
        protected void StartInternal(iTunesLib.IITFileOrCDTrack song)
        {
            this.GetFetchRequestData(song).Status = LyricsFetchStatus.Fetching;

            LyricsFetcher fetcher = new LyricsFetcher();
            fetcher.Sources = this.Sources;
            fetcher.StatusEvent += new EventHandler<LyricsFetchStatusEventArgs>(fetcher_StatusEvent);

            Thread thread = new Thread(new ParameterizedThreadStart(fetcher.FetchSongLyrics));
            thread.IsBackground = true;
            thread.Start(song);
        }

        #endregion

        #region Events

        public event EventHandler<LyricsFetchStatusEventArgs> StatusEvent;

        protected virtual void OnStatusEvent(LyricsFetchStatusEventArgs args)
        {
            if (this.StatusEvent != null)
                this.StatusEvent(this, args);
        }

        #endregion

        #region Event handlers

        private void fetcher_StatusEvent(object sender, LyricsFetchStatusEventArgs e)
        {
            //if (e.Status == FetchStatus.SourceDone && this.GetStatus(e.Track) == FetchStatus.Fetching)
            //    this.RecordAttempt();

            // Remember which source is being checked
            if (e.Status == LyricsFetchStatus.Fetching && this.GetStatus(e.Song) == LyricsFetchStatus.Fetching)
                this.GetFetchRequestData(e.Song).Source = e.LyricsSource;

            // Is this the final event for a fetch that has not already been cancelled
            bool isFetchingDone = e.Status == LyricsFetchStatus.Done && this.GetStatus(e.Song) == LyricsFetchStatus.Fetching;
            if (isFetchingDone) {
                this.GetFetchRequestData(e.Song).Status = LyricsFetchStatus.Done;
                if (this.AutoUpdateLyrics) {
                    try {
                        this.UpdateLyrics(e.Song, e.Lyrics, e.LyricsSource);
                    }
                    catch (COMException) {
                        // Something went wrong
                        //TODO: Figure out a way to report this to the user
                    }
                }
            }

            // Trigger an event while the lyrics fetch has finished but not yet gone
            this.OnStatusEvent(e);

            // Clean up the fetch
            if (isFetchingDone) {
                this.CleanupOne(e.Song);
            }
        }

        private void UpdateLyrics(iTunesLib.IITFileOrCDTrack s, string lyrics, ILyricsSource source)
        {
            lyrics = lyrics.Trim();
            if (s.Lyrics != null && !string.IsNullOrEmpty(s.Lyrics.Trim()))
                return;
            s.Lyrics = lyrics;
            try
            {
                //s.Save();
            }
            catch (Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("Error saving lyrics: " + ex.Message + "\n\nLyrics: " + lyrics);
            }
        }

        #endregion


        /// <summary>
        /// Instances of this class track the progress of request to fetch lyrics
        /// </summary>
        private class FetchRequestData
        {
            public LyricsFetchStatus Status = LyricsFetchStatus.Waiting;
            public ILyricsSource Source;

            // THINK: Do we want to track the thread as well?
            //public Thread Thread;
        }
    }
}
