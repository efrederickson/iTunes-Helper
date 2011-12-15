/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 11/7/2011
 * Time: 10:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesHelper
{
    /// <summary>
    /// Description of ScheduledSync.
    /// </summary>
    public class ScheduledSync : IDisposable
    {
        private Device Device;
        private int Minutes;
        private Timer timer;
        private bool isSyncing = false;
        
        public ScheduledSync(Device device, int minutes)
        {
            this.Device = device;
            this.Minutes = minutes;
            timer = new Timer();
            timer.Interval = minutes * 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (isSyncing)
                return;
            
            isSyncing = true;
            // sync device
            SyncingForm SF = new SyncingForm();
            SF.setMax(Device.iTunesPlaylist.Tracks.Count);
            SF.SetValue(0);
            SF.SetText("Syncing...");
            SF.Show();
            int v = 0;
            foreach (IITFileOrCDTrack track in Device.iTunesPlaylist.Tracks)
            {
                SF.SetText("Syncing " + track.Name + "...");
                // check if empty track
                if (track.Location == "")
                {
                    MessageBox.Show("Dead track: " + track.Name);
                    continue;
                }
                string outputPath="";
                try
                {
                    // format output path
                    outputPath = Device.Drive.Name + "\\Music\\" + MainForm.CleanName(string.IsNullOrEmpty(track.Artist) ? "Unknown Artist" : track.Artist) + "\\" + MainForm.CleanName(string.IsNullOrEmpty(track.Album) ? "Unknown Album" : track.Album) + "\\" + System.IO.Path.GetFileName(track.Location);
                    // write file to drive
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
                    if (System.IO.File.Exists(outputPath)) // check if it already exists
                        System.IO.File.Delete(outputPath); // if so, delete it
                    System.IO.File.Copy(track.Location, outputPath); // copy the file over
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nOutput Path: " + outputPath);
                }
                SF.SetValue(++v); // increase the progress bar
            }
            SF.Close();
            isSyncing = false;
        }
        
        public void Stop()
        {
            timer.Enabled = false;
            timer.Stop();
        }
        
        public void Start()
        {
            timer.Enabled = true;
            timer.Start();
        }
        
        public void Dispose()
        {
            timer.Enabled = false;
            timer.Stop();
            timer.Dispose();
            timer = null;
            this.Device = null;
            this.Minutes = -1;
        }
    }
}
