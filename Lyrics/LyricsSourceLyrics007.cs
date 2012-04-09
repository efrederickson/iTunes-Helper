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
    /// This lyrics provider queries the Lyrics007Provider service for lyrics of a specified song.
    /// </summary>
    public class LyricsSourceLyrics007 : ILyricsSource
    {
        private static readonly string QueryFormat =
            "http://www.lyrics007.com/{0} Lyrics/{1} Lyrics.html";

        private static readonly string pattern =
            "(?<=<br><br><br>)[^<]((.|\n)*)(?=<br><br><script)";

        public string Name
        {
            get
            {
                return "Lyrics007";
            }
        }


        /// <summary>
        /// Retrieve the lyrics for the given song
        /// </summary>
        /// <param name="song">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>

        public string GetLyrics(iTunesLib.IITFileOrCDTrack song)
        {
            // clean the title; we don't need quotes
            string title = Regex.Replace(song.Name, "['\"]", "");

            string lyrics = String.Empty;

            using (WebClient client = new WebClient())
            {
                try
                {
                    string uri = Uri.EscapeUriString(
                        String.Format(QueryFormat, song.Artist, song.Name));

                    string result = client.DownloadString(uri);

                    if (!String.IsNullOrEmpty(result))
                    {
                        Match match = Regex.Match(result, pattern, RegexOptions.IgnoreCase);

                        if (match.Success)
                        {
                            lyrics = Flatten(match.Value);
                            lyrics = Unbreak(lyrics);
                            lyrics = Encode(lyrics);
                        }
                    }
                }
                catch (Exception)
                {
                    lyrics = String.Empty;
                }
            }

            return lyrics;
        }


        /// <summary>
        /// Clean up the lyrics and encode into Unicode to preserve special characters.
        /// </summary>
        /// <param name="lyrics"></param>
        /// <returns></returns>

        protected string Encode(string text)
        {
            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
            return Encoding.UTF8.GetString(encoding.GetBytes(text.Trim()));
        }


        /// <summary>
        /// Remove all carriage return and vertical line feed characters from the
        /// string so we can format it ourselves using BR (HTML Break) hints.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        protected string Flatten(string text)
        {
            return text.Replace("\r", String.Empty).Replace("\n", String.Empty);
        }


        /// <summary>
        /// Replace all instances of BR tags (HTML Break) with proper NewLine sequences.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        protected string Unbreak(string text)
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
    }
}
