/* Rename me to EvilLyricsImport.js
   
Double Click in Explorer to run 	
Script by Otto - http://ottodestruct.com       */




// set this to the path to EvilLyrics's lyrics directory

var lyricsdir = "c:\\program files\\evillyrics\\lyrics\\";

// set this to true to update all lyrics (instead of just newly found lyrics)

var updateAll = false;







/*

 Don't change anything after this line unless you know what you are doing.

*/


var ITTrackKindFile = 1;

var iTunesApp = WScript.CreateObject("iTunes.Application"); 

var tracks = iTunesApp.LibraryPlaylist.Tracks;

var numTracks = tracks.Count;

var i;

var fso = new ActiveXObject("Scripting.FileSystemObject");


var count = 0;

for (i = 1; i <= numTracks; i++) 

{
    var currTrack = tracks.Item(i);

    // got lyrics?

    if (currTrack.Kind == ITTrackKindFile && ! currTrack.Podcast )

    {
        try

        {

            if (currTrack.Lyrics == "" || updateAll == true)

            {

                // figure out the filename to look for

                tempartist = currTrack.Artist;

                tempname = currTrack.Name;

                // kill everything after the period (disabled)

                //tempartist = tempartist.replace(/[\.].*/,'');

                //tempname = tempname.replace(/[\.].*/,'');

                // replace single quotes with underscores

                tempartist = tempartist.replace(/[\']+/g,'_');

                tempname = tempname.replace(/[\']+/g,'_');


                // kill everything in parenths or brackets

                tempartist = tempartist.replace(/[\(\[].*[\)\]]/g,'');

                tempname = tempname.replace(/[\(\[].*[\)\]]/g,'');


                // kill multiple spaces
                tempartist = tempartist.replace(/[ ]+/g,' ');

                tempname = tempname.replace(/[ ]+/g,' ');


                // just to be sure

                tempartist = tempartist.replace(/[ ]+/g,' ');

                tempname = tempname.replace(/[ ]+/g,' ');
                // kill leading and trailing whitespace

                tempartist = tempartist.replace(/^\s+|\s+$/g,'');

                tempname = tempname.replace(/^\s+|\s+$/g,'');


                // anything else that ain't a alnum or period becomes an underscore

                tempartist = tempartist.replace(/[^0-9a-zA-ZÀàáäãâçÉéèêÔöôóóõüúùÍíiñ\. ]+/g,'_');

                tempname = tempname.replace(/[^0-9a-zA-ZÀàáäãâçÉéèêÔöôóóõüúùÍíiñ\. ]+/g,'_');

                var fullpath = lyricsdir + tempartist.charAt(0) + "\\" + tempartist + " - " + tempname + ".txt";


                if (fso.FileExists(fullpath))

                {

                // it's there, read it

                var tf = fso.OpenTextFile(fullpath,1);

                var mylyrics = tf.ReadAll();

                tf.Close();

                // eliminate the footer

                mylyrics = mylyrics.replace(/----------------.*[\r\n]+.*[\r\n]+.*/,'');


                // add 'em

                currTrack.Lyrics = mylyrics;

                count++;

                }

            }

        }

        catch(er)

        {

	    // ignore these sorts of errors

        }

    }
} 


WScript.Echo ("Added Lyrics to " + count + " tracks");