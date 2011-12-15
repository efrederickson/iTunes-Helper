' ###############################################################################
' #
' # itunes_insert_artwork.vbs
' #
' # This script will tag your files using artwork downloaded using iTunes 7
' #
' # written by: Robert Jacobson (http://mysite.verizon.net/teridon/itunesscripts)
' # Last Updated: 03 Jan 2007
' # Version 1.0
' #
' # This script is GPL v2.  see http://www.gnu.org/copyleft/gpl.html
' #
' # Use option "-k" to keep the artwork files extracted 
' # (in the same location as the song file)
' # (the default is to remove the files)
' ###############################################################################

Option Explicit

Dim iTunesApp        ' iTunes.Application object used to access the iTunes application.
Dim tracks           ' The tracks collection object of the Library object. 
Dim TrackPath        ' The path to the track
Dim ArtPath	         ' The path to the artwork
Dim i                ' A counter variable.
Dim Msg              ' A string for a message.
Dim f                ' A file object.
Dim sources
Dim source
Dim playlists
Dim playlist
Dim playlistName
Dim j
Dim m
Dim c
Dim songName
Dim artist
Dim result
Dim listarray
Dim num
Dim k
Dim track
Dim numtracks
Dim FormatArray(4)
Dim ExtArray(4)
Dim Artobj
Dim Art
Dim ArtDir
Dim Format
Dim BasePath
Dim fso 
Dim NumFiles
Dim KeepFiles
Dim args
Dim arg

Set fso = CreateObject("Scripting.FileSystemObject")

FormatArray(0) = "Unknown"
FormatArray(1) = "JPEG"
FormatArray(2) = "PNG"
FormatArray(3) = "BMP"
ExtArray(0) = "unk"
ExtArray(1) = "jpg"
ExtArray(2) = "png"
ExtArray(3) = "bmp"

Set iTunesApp  = CreateObject("iTunes.Application.1")
Set sources = iTunesApp.Sources

Dim vers
vers = iTunesApp.Version

'Dim Reg1
'Set Reg1 = new RegExp
'Reg1.Pattern = "^7"
'if Reg1.Test(vers) Then
'	' yay
'Else
'	Wscript.Echo "This script requires iTunes 7"
'	Wscript.Quit
'End If
KeepFiles = False

Set args = WScript.Arguments
' Scan command line arguments
For Each arg in args 
  ' Is it a flag.
  If Instr(1, arg, "-", 1) = 1 or Instr(1, arg, "/", 1) = 1 Then
    ' Check for list flag
    If UCase(arg) = "-K" or UCase(arg) = "/K" then
 KeepFiles = True
    End If
  End If
Next

For i = 1 to sources.Count
	Set source = sources.Item(i)
	
	IF source.Kind = 1 Then
		Set playlists = source.Playlists
		Wscript.Echo "Select from the following playlists" & chr(13) & chr(10)
		For j = 1 to playlists.Count
			Set playlist = playlists.Item(j)
			playlistName = playlist.Name
			Wscript.Echo j & ": " & playlistName
		Next
		Wscript.Echo ""
		Wscript.StdOut.Write "Enter comma-separated lists to process: "
		result = Wscript.StdIn.ReadLine
		
 		listarray = split(result, ",")
 		For k = 0 to UBound(listarray)
 			num = listarray(k)
 		   	Set playlist = playlists.Item(num)
 		   	playlistName = playlist.Name
 		   	Wscript.Echo ""
 		   	Wscript.Echo chr(9) & "Processing playlist " & num & ": " & playlistName
 		   	
 		   	Set tracks = playlist.Tracks
 		   	numtracks = tracks.Count
 		   	Wscript.Echo chr(9) & "tracks: " & numtracks
 		   	NumFiles = 0
 		   	For m = 1 to numtracks
 		   		If m > tracks.Count Then Exit For
 		   		Set track = tracks.Item(m)
 		   		'Wscript.Echo "num: " & numtracks & " Count: " & tracks.Count & " m: " & m
 		   		
 		   		If track.Kind = 1 Then
 		   			songName = track.Name
 		   			artist = track.Artist
 		   			TrackPath = track.Location
 		   			Set Artobj = track.Artwork
					For c = 1 to Artobj.Count
						Set Art = Artobj.Item(c)
						if Art.IsDownloadedArtwork Then
							Format = Art.Format
							'Wscript.Echo "Format is " & FormatArray(Format)
							ArtDir = fso.GetParentFolderName(TrackPath)
							'Wscript.Echo "Artdir is " & ArtDir
							'ArtDir = fso.GetBaseName(ArtDir)
							Dim RegX
							Set RegX = new RegExp
							RegX.Pattern = "[/:\\\*\?""""<>]"
							RegX.Global = True
							songName = RegX.Replace(songName, "-")
							'songName = Replace(songName, "/", "-")
							ArtPath = fso.BuildPath(ArtDir, songName & "." & ExtArray(Format))
							Wscript.Echo "artpath is " & ArtPath
							' save to file
							Art.SaveArtworkToFile(ArtPath)
							' insert from file into track tag
							Art.SetArtworkFromFile(ArtPath)
							if (KeepFiles) Then
								' nothing
							Else
								fso.DeleteFile(ArtPath)
							End If
							NumFiles = NumFiles + 1
						End If
					Next
 		   			
 		   		End If
 		   	Next
 		   	Wscript.Echo NumFiles & " files processed in playlist " & playlistName
 		Next
 		
		'End If
	End If
Next
