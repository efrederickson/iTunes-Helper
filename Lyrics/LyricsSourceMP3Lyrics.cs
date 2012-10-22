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
    /// This lyrics provider queries the MP3Lyrics.org service for lyrics of a specified song.
    /// </summary>
    public class LyricsSourceMP3Lyrics : ILyricsSource
    {
        private static readonly string QueryFormat = "http://www.mp3lyrics.org/{0}/{1}/{2}/";

        private static readonly string pattern =
            "(?<=<div id=\"lyrics_text\".*?>)((.|\n)*?)(?=</div>)";

        private static readonly string advertisingPattern = "(<span.*/span>)";

        public string Name
        {
            get
            {
                return "MP3Lyrics";
            }
        }

        /// <summary>
        /// Retrieve the lyrics for the given song
        /// </summary>
        /// <param name="song">The song whose lyrics are to be fetched</param>
        /// <returns>The lyrics or an empty string if the lyrics could not be found</returns>
        public string GetLyrics(iTunesLib.IITFileOrCDTrack song)
        {
            if (string.IsNullOrEmpty(song.Artist) || string.IsNullOrEmpty(song.Name))
                return "";
            // clean the title; we don't need quotes
            string title = song.Name.Replace("\"", "");

            string lyrics = String.Empty;

            using (WebClient client = new WebClient())
            {
                try
                {
                    string category = song.Artist.Substring(0, 1);
                    if (char.IsNumber(category, 0))
                        category = "0-9";

                    string artist = song.Artist.ToLower().Replace(" ", "-");
                    title = title.ToLower().Replace(" ", "-").Replace("'", "-");

                    string uri = Uri.EscapeUriString(
                        String.Format(QueryFormat, category, song.Artist, title));

                    string result = client.DownloadString(uri);

                    if (!String.IsNullOrEmpty(result))
                    {
                        Match match = Regex.Match(result, pattern);

                        if (match.Success)
                        {
                            lyrics = match.Value;
                            match = Regex.Match(lyrics, advertisingPattern);
                            if (match.Success)
                            {
                                var builder = new StringBuilder(lyrics);
                                lyrics = builder.Remove(match.Index, match.Length).ToString();
                            }

                            lyrics = Unbreak(lyrics)
                                .Replace("<i/>", String.Empty)
                                .Replace("<i>", String.Empty)
                                .Replace("<u/>", String.Empty)
                                .Replace("<u>", String.Empty);

                            lyrics = Encode(lyrics);
                        }
                    }
                }
                catch (Exception ex)
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
