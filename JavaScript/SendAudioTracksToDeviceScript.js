function getAudioCDSource() {
  return getSource(3);
}
function getDeviceSource() {
  return getSource(2);
}

function getSource(kind) {
  var sources = iTunesApp.Sources;
  
  for (var i = 1; i <= sources.Count; i++) {
    var s = sources.Item(i);
    if (s.Kind == kind) return s;
  }
}

function echo(str) {
  delLast();
  WScript.Echo(str);
  lastLen = 0;
}


var lastLen = 0;
function echo2(str) {
  delLast();
  WScript.StdOut.Write(str);
  lastLen = str.length;
}

function delLast() {
  var str = "";
  for (var i = 0; i < lastLen; i++) {
    str = bs() + str;
  }  
  WScript.StdOut.Write(str);
}

function bs() {
  return String.fromCharCode(8);
}

function waitForProgress(prog) {
  var tr = null;
  while ( prog.InProgress ) {
    var percent = Math.round( prog.ProgressValue / prog.MaxProgressValue * 100);
    
    if (tr != prog.TrackName) {
      echo(prog.TrackName);
      tr = prog.TrackName;
    }
    
    echo2(percent + "%");
    WScript.Sleep(500);
  }
}

function waitFor(prog) {
  while (prog.InProgress) {
    WScript.Sleep(500);
  }
}

function getArg(argindex, defaultval) {
  var val = defaultval;

  if (argindex < WScript.Arguments.length) {
    val = WScript.Arguments(argindex);
  }
  
  return val;
}

WScript.Interactive = true;
var fso = new ActiveXObject("Scripting.FileSystemObject");
var	iTunesApp = WScript.CreateObject("iTunes.Application");
var list = getAudioCDSource().Playlists.Item(1);
var devicePlaylist = getDeviceSource().Playlists.Item(1);
var mainPlaylist = iTunesApp.LibraryPlaylist;

var startTrackNum = getArg(0, 1);
var endTrackNum = getArg(1, list.Tracks.Count);
var arImportedTracks = new Array;
var i;

// First pass: Import from CD
/*
for (i = startTrackNum; i <= endTrackNum; i++) {

  // Import audio cd track into library
  var track = list.Tracks.Item(i);
  var prog = iTunesApp.ConvertTrack2(track);
  echo("Importing \"" + prog.TrackName + "\" (Track " + i + ")... ");
  waitForProgress(prog);

  // Add to array of imported tracks
  arImportedTracks.push(prog.Tracks.Item(1));
}
*/

echo("Importing from Audio CD");
var prog = iTunesApp.ConvertTracks2(list.Tracks);
waitForProgress(prog);

for (i = 1; i <= prog.Tracks.Count; i++) {
  arImportedTracks.push(prog.Tracks.Item(i));
}

// Second pass: upload to device
for (i = 0; i < arImportedTracks.length; i++) {
  var trackToTransfer = arImportedTracks[i];
  echo("\nSending track to device: " + trackToTransfer.Name);
  
  // Upload new track from library to device
  echo("...transferring");
  var prog = devicePlaylist.AddFile(trackToTransfer.Location);
  waitFor(prog);
  
  // Delete from Library
  echo("...removing from Library");
  var filePath = trackToTransfer.Location;
  trackToTransfer.Delete();
  
  // Delete from computer's hard drive
  echo("...deleting \"" +fso.GetFileName(filePath) + "\" from computer");
  fso.DeleteFile(filePath);
}

