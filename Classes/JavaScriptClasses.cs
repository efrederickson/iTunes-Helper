using System;
using iTunesLib;
using System.Collections;
using System.Collections.Generic;

namespace iTunesHelper.Classes
{
    public class AdjustVolume
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the amount of tracks changed</returns>
        public static int Run()
        {
            IITTrackCollection tracks = MainForm.iTunes.LibraryPlaylist.Tracks;
            int numTracks = tracks.Count;
            int changedTracks = 0;
            int i;
            for (i = 0; i <= numTracks; i++)
            {
                var currTrack = tracks[i];
                // is this a file track?
                if (currTrack.Kind == ITTrackKind.ITTrackKindFile)
                {
                    try
                    {
                        if ("Podcast" == currTrack.Genre || "Audiobook" == currTrack.Genre)
                        {
                            currTrack.EQ = "Spoken word";
                            currTrack.VolumeAdjustment = 100;
                            changedTracks++;
                        }
                    }
                    catch (Exception er)
                    {
                        // ignore these errors
                        Console.WriteLine(er);
                    }
                }
            }
            return changedTracks;
        }
    }

    public class ClearLyrics
    {
        public static void Run()
        {
            IITTrackCollection tracks = MainForm.iTunes.LibraryPlaylist.Tracks;
            int numTracks = tracks.Count;
            int i;
            for (i = 1; i <= numTracks; i++)
            {
                IITFileOrCDTrack currTrack = tracks[i] as IITFileOrCDTrack;
                // is this a file track?
                if (currTrack.Kind == ITTrackKind.ITTrackKindFile)
                {
                    try
                    {
                        if (currTrack.Lyrics != "")
                            currTrack.Lyrics = "";
                    }
                    catch (Exception)
                    {
                        // ignore these errors
                    }
                }
            }
        }
    }

    public class UpdateSongInfoFromFile
    {
        public static void Run()
        {
            foreach (IITFileOrCDTrack track in MainForm.iTunes.LibraryPlaylist.Tracks)
                track.UpdateInfoFromFile();
        }
    }

    /*public class NoArtPlaylist
    {
    public static void Run()
    {
    IITPlaylist _NoArtPlaylist = MainForm.iTunes.CreatePlaylist("No Artwork");
    foreach (IITFileOrCDTrack track in MainForm.iTunes.LibraryPlaylist.Tracks)
    if (track.Artwork.Count == 0) 
    _NoArtPlaylist.AddTrack(track);
    }
    }*/

    /*public class CreateAlbumPlaylists
    {
        public static int Run()
        {
            IITSource mainLibrarySource = MainForm.iTunes.LibrarySource;
            int numPlaylistsCreated = 0;

            // first, make an array indexed by album name
            Hashtable albumArray = new Hashtable();

            foreach (IITFileOrCDTrack track in MainForm.iTunes.LibraryPlaylist.Tracks)
            {
                string album = track.Album;
    
                if (!string.IsNullOrEmpty(album) && album != null)
                {
                    if (albumArray[album] == null)
                    {
                        Console.WriteLine("Adding album " + album);
                        albumArray[album] = new Stack();
                    }
        
                    // add the track to the entry for this album
                    ((Stack) albumArray[album]).Push(track);
                }
            }

            foreach (string albumNameKey in albumArray)
            {
                IITPlaylist albumPlaylist;
                Stack trackArray = (Stack) albumArray[albumNameKey];
                Console.WriteLine("Creating playlist " + albumNameKey);
                numPlaylistsCreated++;
                albumPlaylist = MainForm.iTunes.CreatePlaylist(albumNameKey);

                for (int trackIndex = 0; trackIndex < trackArray.Count; trackIndex++)
                {
                    IITFileOrCDTrack currTrack = (IITFileOrCDTrack)trackArray.Pop();
                    Console.WriteLine("   Adding " + currTrack.Name);
                    albumPlaylist.AddTrack(currTrack);
                }
            }

            return numPlaylistsCreated;
        }
    }*/
}