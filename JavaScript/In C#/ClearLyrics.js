/*
	Rename me to ClearLyrics.js
	Double-Click in Explorer to run 	


	WARNING: This is not undoable. Use with Caution.

Script by Otto - http://ottodestruct.com       

*/

var ITTrackKindFile = 1;
var iTunesApp = WScript.CreateObject("iTunes.Application");
var tracks    = iTunesApp.LibraryPlaylist.Tracks;
var numTracks = tracks.Count;
var i;
for (i = 1; i <= numTracks; i++)
{
	var currTrack = tracks.Item(i);
	// is this a file track?
	if (currTrack.Kind == ITTrackKindFile)
	{
	
	try {
		if (currTrack.Lyrics != "") 
		{
			currTrack.Lyrics = "";
		}
	}
	catch(er)
	{
	// ignore these errors
	}
	
	}
}
WScript.Echo("All Lyrics Cleared!")