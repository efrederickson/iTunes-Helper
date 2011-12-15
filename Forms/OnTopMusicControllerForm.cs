/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 12/15/2011
 * Time: 10:13 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using iTunesLib;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// Description of OnTopMusicControllerForm.
    /// </summary>
    public partial class OnTopMusicControllerForm : Form
    {
        public OnTopMusicControllerForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        void OnTopMusicControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Instance.button9.Enabled = true;
        }
        
        public void Play()
        {
            MainForm.iTunes.Play();
        }

        public void Pause()
        {
            MainForm.iTunes.Pause();
        }

        public void Rewind()
        {
            try
            {
                if (MainForm.iTunes.PlayerPosition > 3)
                {
                    MainForm.iTunes.PlayerPosition = 0;
                }
                else
                {
                    PreviousTrack();
                }
            }
            catch (Exception)
            {
            }
        }

        public void PreviousTrack()
        {
            MainForm.iTunes.PreviousTrack();
        }

        public void NextTrack()
        {
            MainForm.iTunes.NextTrack();
        }

        public bool IsPlaying
        {
            get
            {
                try
                {
                    return MainForm.iTunes.PlayerState == ITPlayerState.ITPlayerStatePlaying;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        
        void Button1_Click(object sender, EventArgs e)
        {
            Rewind();
        }
        
        void Button2_Click(object sender, EventArgs e)
        {
            NextTrack();
        }
        
        void Button3_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
                Pause();
            else
                Play();
        }
        
        void Button4_Click(object sender, EventArgs e)
        {
            MainForm.iTunes.Stop();
        }
        
        void Timer1_Tick(object sender, EventArgs e)
        {
            if (IsPlaying)
                button3.Text = "Pause";
            else
                button3.Text = "Play";
            
            if (MainForm.iTunes.CurrentTrack != null)
                label1.Text = MainForm.iTunes.CurrentTrack.Name;
            else
                label1.Text = "No Track is Playing!";
        }
    }
}
