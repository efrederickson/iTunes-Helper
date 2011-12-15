/* 	Rename me to NoLyricsPlaylist.js
	Double Click in Explorer to run

Script by Otto - http://ottodestruct.com       */

var iTunesApp = WScript.CreateObject("iTunes.Application"); 
var tracks = iTunesApp.LibraryPlaylist.Tracks;
var numTracks = tracks.Count;
var i;
NoLyricsPlaylist = iTunesApp.CreatePlaylist("No Lyrics");

for (i = 1; i <= numTracks; i++) 
{ 	try {
	var currTrack = tracks.Item(i); 
	if ( currTrack.Lyrics == "" ) 
		NoLyricsPlaylist.AddTrack(currTrack);
	}
	catch(er)
	{
	}
} 

