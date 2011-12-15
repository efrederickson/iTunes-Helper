/*
 * Adjust volume and eq setting of podcasts and audiobooks
 * http://rubenlaguna.com/wp
 * Jscript file must be run with Windows wscript.exe  
 * Based on a script by Otto - http://ottodestruct.com       
 */

var iTunesApp = WScript.CreateObject("iTunes.Application");

var ITTrackKindFile = 1;
var tracks    = iTunesApp.LibraryPlaylist.Tracks;
var numTracks = tracks.Count;
var changedTracks = 0;
var i;
for (i = 1; i <= numTracks; i++)
{
	var currTrack = tracks.Item(i);
	// is this a file track?
	if (currTrack.Kind == ITTrackKindFile)
	{
		try {
			if ("Podcast" == currTrack.Genre || "Audiobook" == currTrack.Genre) {
				currTrack.EQ = "Spoken word";
				currTrack.VolumeAdjustment = 100;
				changedTracks++;
			}
		}
		catch(er)
		{
			// ignore these errors
			WScript.Echo(er);
		}
	}
}
//WScript.Echo(changedTracks + " track changed");
//iTunesApp.Stop();
