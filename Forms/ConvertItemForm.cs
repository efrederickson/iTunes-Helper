/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 11/7/2011
 * Time: 9:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iTunesHelper.Forms
{
    /// <summary>
    /// Description of ConvertItemForm.
    /// </summary>
    public partial class ConvertItemForm : Form
    {
        public ConvertItemForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "All Files|*.*";
            o.Multiselect = true;
            if (o.ShowDialog() == DialogResult.OK)
            {
                foreach (string fn in o.FileNames)
                {
                    listView1.Items.Add(new ListViewItem(System.IO.Path.GetFileNameWithoutExtension(fn)) { Tag = fn });
                }
            }
        }
        
        void Button2_Click(object sender, EventArgs e)
        {
            foreach ( ListViewItem i in listView1.SelectedItems)
            {
                listView1.Items.Remove(i);
            }
        }
        
        void Button3_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            foreach (ListViewItem i in listView1.Items)
                l.Add(i.Tag as string);
            
            MainForm.iTunes.ConvertFiles(l.ToArray());
        }
    }
}
