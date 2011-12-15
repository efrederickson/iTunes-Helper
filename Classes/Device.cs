using System;
using System.Collections.Generic;
using System.Linq;
using iTunesLib;
using System.IO;

namespace iTunesHelper
{
    public class Device
    {
        private IITPlaylist _iTunesPlaylist;
        public IITPlaylist iTunesPlaylist
        {
            get
            {
                return _iTunesPlaylist;
            }
            set
            {
                _iTunesPlaylist = value;
            }
        }

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        public DriveInfo Drive;

        public Device(IITPlaylist playlist, DriveInfo device)
        {
            _iTunesPlaylist = playlist;
            Drive = device;
            Name = device.Name;
        }
    }
}