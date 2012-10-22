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
    /// Search AbsoluteLyrics.com for lyrics
    /// </summary>
    public class LyricsSourceAbsoluteLyrics : ILyricsSource
    {
        // This one was written by LoDC

        public string Name
        {
            get { return "Absolute Lyrics"; }
        }

        // http://www.absolutelyrics.com/lyrics/view/i_see_stars/gnars_attacks
        private const string pattern = "(?<=<p id=\"view_lyrics\">)((.|\n)*)(?=</p>)";

        public string GetLyrics(iTunesLib.IITFileOrCDTrack s)
        {
            if (string.IsNullOrEmpty(s.Artist) || string.IsNullOrEmpty(s.Name))
                return "";
            string ret = String.Empty;
            string url = string.Format("http://www.absolutelyrics.com/lyrics/view/{0}/{1}",
                                       s.Artist.Replace(" ", "_"), s.Name.Replace(" ", "_"));
            try
            {
                WebClient wc = new WebClient();
                string html = wc.DownloadString(url);
                Match match = Regex.Match(html, pattern);
                if (match.Success)
                {
                    ret = Flatten(match.Value);

                    ret = Unbreak(ret)
                        .Replace("<i>", "")
                        .Replace("</i>", "");

                }
            }
            catch (Exception ex)
            {
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
