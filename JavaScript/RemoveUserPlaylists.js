var ITPlaylistKindUser = 2;
var	iTunesApp = WScript.CreateObject("iTunes.Application");
var	deletedPlaylists = 0;
var	mainLibrary = iTunesApp.LibrarySource;
var	playlists = mainLibrary.Playlists;
var	numPlaylists = playlists.Count;

while (numPlaylists != 0)
{
	var	currPlaylist = playlists.Item(numPlaylists);
	
	// is this a user playlist?
	if (currPlaylist.Kind == ITPlaylistKindUser)
	{
		// yes, is it a dumb playlist?
		if (!currPlaylist.Smart)
		{
			try
			{
				// yes, delete it
				currPlaylist.Delete();
				deletedPlaylists++;
			}
			catch (exception)
			{
				// ignore errors (e.g. trying to delete a locked playlist)
			}
		}
	}
	
	numPlaylists--;
}

if (deletedPlaylists > 0)
{
	if (deletedPlaylists == 1)
	{
		WScript.Echo("Removed 1 user playlist.");
	}
	else
	{
		WScript.Echo("Removed " + deletedPlaylists + " user playlists.");
	}
}
else
{
	WScript.Echo("No user playlists were removed.");
}
