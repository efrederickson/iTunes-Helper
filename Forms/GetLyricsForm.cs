/*
 * User: elijah
 * Date: 4/20/2012
 * Time: 2:15 PM
 * Copyright 2012 LoDC
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// Description of GetLyricsForm.
    /// </summary>
    public partial class GetLyricsForm : Form
    {
        public GetLyricsForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            label2.Text = "";
            label1.Text = "";
            t = new MediaPlayer.Tasks.LyricsFetchingTask(MainForm.Library);
            t.OnStatusChanged += new EventHandler(t_OnStatusChanged);
            t.OnDone += delegate { Close(); };
            t.Run();
        }
        MediaPlayer.Tasks.LyricsFetchingTask t = null;

        void t_OnStatusChanged(object sender, EventArgs e)
        {
            Invoke(new EventHandler(delegate{
                                        label1.Text = t.Text;
                                        label2.Text = t.SubText;
                                        progressBar1.Maximum = t.TotalAmount;
                                        progressBar1.Value = t.CurrentProgressAmount;
                                    }));
        }
    }
}
