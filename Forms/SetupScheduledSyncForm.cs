/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 11/7/2011
 * Time: 10:25 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// Description of SetupScheduledSyncForm.
    /// </summary>
    public partial class SetupScheduledSyncForm : Form
    {
        private Device device;
        public ScheduledSync result;
        public SetupScheduledSyncForm(Device d)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            this.device = d;
            this.label1.Text = "Setup Sync Schedule for " + d.Name;
        }
        
        void Button1_Click(object sender, EventArgs e)
        {
            try {
                int.Parse(textBox1.Text);
            } catch (Exception) {
                MessageBox.Show("Cannot parse Text! Please try again");
                return;
            }
            result = new ScheduledSync(device, int.Parse(textBox1.Text));
            Close();
        }
    }
}
