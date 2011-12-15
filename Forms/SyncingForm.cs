using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iTunesHelper
{
    public partial class SyncingForm : Form
    {
        public SyncingForm()
        {
            InitializeComponent();
        }

        public void SetText(string t)
        {
            label1.Text = t;
        }

        public void SetValue(int value)
        {
            progressBar1.Value = value;
        }

        public void setMax(int max)
        {
            progressBar1.Maximum = max;
        }
    }
}
