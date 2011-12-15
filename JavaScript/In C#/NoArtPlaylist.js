/* 	Rename me to NoArtPlaylist.js
	Double Click in Explorer to run

Script by Otto - http://ottodestruct.com       */

var iTunesApp = WScript.CreateObject("iTunes.Application"); 
var tracks = iTunesApp.LibraryPlaylist.Tracks;
var numTracks = tracks.Count;
var i;
NoArtPlaylist = iTunesApp.CreatePlaylist("No Artwork");

for (i = 1; i <= numTracks; i++) 
{ 	
	var currTrack = tracks.Item(i); 
	if ( currTrack.Artwork.Count == 0 ) 
		NoArtPlaylist.AddTrack(currTrack);
} 

