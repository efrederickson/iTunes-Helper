using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace iTunesHelper
{
    /// <summary>
    /// This source tries to fetch song lyrics from the LyricsPlugin site (www.lyricsplugin.com)
    /// </summary>
    public class LyricsSourceLyricsPlugin : ILyricsSource
    {
        /// <summary>
        /// Gets the name of this source
        /// </summary>
        public string Name
        {
            get
            {
                return "LyricsPlugin";
            }
        }

        /// <summary>
        /// Fetch the lyrics for the given song
        /// </summary>
        /// <param name="s">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>
        public string GetLyrics(iTunesLib.IITFileOrCDTrack s)
        {
            WebClient client = new WebClient();
            string queryUrl = "http://www.lyricsplugin.com/plugin/?title=" + s.Name + "&artist=" + s.Artist;
            try
            {
                string result = client.DownloadString(queryUrl);

                const string lyricsMarker = @"<div id=""lyrics"">";
                int index = result.IndexOf(lyricsMarker);
                if (index >= 0)
                {
                    result = result.Substring(index + lyricsMarker.Length);
                    index = result.IndexOf("</div>");
                    if (index > 0)
                    {
                        result = result.Substring(0, index);
                        result = result.Replace("<br />\n", Environment.NewLine);
                        result = result.Replace("<br />", "");
                        result = result.Replace("â€™", "'");
                        Encoding latin1 = Encoding.GetEncoding("ISO-8859-1");
                        result = Encoding.UTF8.GetString(latin1.GetBytes(result));
                        return result.Trim();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("LyricsPlugin GetLyrics failed:");
                System.Diagnostics.Debug.WriteLine(ex);
                return String.Empty;
            }

            return String.Empty;
        }
    }
}
