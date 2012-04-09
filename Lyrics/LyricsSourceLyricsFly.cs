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
    /// This source tries to fetch song lyrics from the LyricsFly site (www.lyricsfly.com)
    /// </summary>
    public class LyricsSourceLyricsFly : ILyricsSource
    {
        // Originally from LyricsFetcher, but it was broken. 
        // LoDC fixed it.

        /// <summary>
        /// Gets the name of this source
        /// </summary>
        public string Name
        {
            get
            {
                return "LyricsFly";
            }
        }

        /// <summary>
        /// Fetch the lyrics for the given song
        /// </summary>
        /// <param name="s">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>
        public string GetLyrics(iTunesLib.IITFileOrCDTrack s)
        {

            string pattern = "(?<=<span class=\"adbriteinline\">)((.|\n)*)(?=</span>)";

            string ret = "";
            string queryUrl = String.Format("http://lyricsfly.com/search/view.php?7d588db3aa&view=606854&artist={0}&title={1}&hl= &opt=main",
                                            s.Artist, s.Name);

            try
            {

                WebClient client = new WebClient();
                string result = client.DownloadString(queryUrl);

                Match match = Regex.Match(result, pattern);
                if (match.Success)
                {
                    ret = Flatten(match.Value.Substring(0, match.Value.LastIndexOf("</span>")));

                    ret = Unbreak(ret)
                        .Replace("<i>", "")
                        .Replace("&#039;", "'")
                        .Replace("</i>", "");

                }
            }
            catch (XmlException ex)
            {
                // Not much we can do here
                Console.WriteLine(ex.ToString());
                ret = "";
            }
            return ret;
        }


        string Unbreak(string text)
        {
            MatchCollection matches = Regex.Matches(
                text, @"(<\s*br\s*/?\s*>)", RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                var builder = new StringBuilder(text);
                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    builder.Remove(matches[i].Index, matches[i].Length);
                    builder.Insert(matches[i].Index, Environment.NewLine);
                }

                return builder.ToString();
            }
            return text;
        }

        string Flatten(string text)
        {
            return text.Replace("\r", String.Empty).Replace("\n", String.Empty);
        }
    }
}
