using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace MediaPlayer.Lyrics
{
    /// <summary>
    /// This source tries to fetch song lyrics from the Lyrdb site (www.lyrdb.com)
    /// </summary>
    public class LyricsSourceLyrdb : ILyricsSource
    {
        const string PROGRAM_NAME = "Lydb";
        const string PROGRAM_VERSION = "0.6.1";

        /// <summary>
        /// Gets the name of this source
        /// </summary>
        public string Name
        {
            get
            {
                return "Lyrdb";
            }
        }

        /// <summary>
        /// Fetch the lyrics for the given song
        /// </summary>
        /// <param name="s">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>
        public string GetLyrics(iTunesLib.IITFileOrCDTrack s)
        {
            if (string.IsNullOrEmpty(s.Artist) || string.IsNullOrEmpty(s.Name))
                return "";
            // Lyrdb can't handle single or double quotes in the title (as of 12/1/2008)
            // So we just remove them
            string title = s.Name.Replace("'", "");
            title = title.Replace("\"", "");

            WebClient client = new WebClient();
            string result = String.Empty;

            try
            {
                // If we have both the title and the artist, we can look for a perfect match
                if (!String.IsNullOrEmpty(s.Artist))
                {
                    string queryUrl = String.Format("http://webservices.lyrdb.com/lookup.php?q={0}|{1}&for=match&agent={2}/{3}",
                        s.Artist, title, PROGRAM_NAME, PROGRAM_VERSION);
                    result = client.DownloadString(queryUrl);
                }

                // If we only have the title or the perfect match failed, do a full text search using 
                // whatever information we have
                //if (result == String.Empty)
                //{
                //    string queryUrl = String.Format("http://webservices.lyrdb.com/lookup.php?q={0}&for=trackname&agent={2}/{3}",
                //        title, s.Artist, PROGRAM_NAME, PROGRAM_VERSION);
                //    result = client.DownloadString(queryUrl);
                //}
            }
            catch (Exception ex)
            {
                // Many things can do wrong here, but there's nothing we can do it fix them
                System.Diagnostics.Debug.WriteLine("Lyrdb GetLyrics failed:");
                System.Diagnostics.Debug.WriteLine(ex);
                return String.Empty;
            }

            // Still didn't work? Give up.
            if (result == String.Empty || result.StartsWith("error:"))
                return String.Empty;

            foreach (string x in result.Split('\n'))
            {
                string id = x.Split('\\')[0];
                try
                {
                    string lyrics = client.DownloadString("http://webservices.lyrdb.com/getlyr.php?q=" + id);
                    if (lyrics != String.Empty && !lyrics.StartsWith("error:"))
                    {
                        Encoding latin1 = Encoding.GetEncoding("ISO-8859-1");
                        string str = Encoding.UTF8.GetString(latin1.GetBytes(lyrics));
                        str = str.Replace("\r", String.Empty);
                        return str.Replace("\n", System.Environment.NewLine);
                    }
                }
                catch (Exception)
                {

                }

            }

            return String.Empty;
        }
    }
}
