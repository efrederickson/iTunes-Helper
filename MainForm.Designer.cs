/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 10/25/2011
 * Time: 11:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace iTunesHelper
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUpNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitoniTunesCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitITunesHelperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.scheduledSyncsListBox = new System.Windows.Forms.ListBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.MWCCheckBox = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.button16 = new System.Windows.Forms.Button();
            this.notifyIconContextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Scan for Duplicate Files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(19, 86);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(147, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Verify Library Files";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Scan for Missing Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(19, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Move Library (not reliable)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(40, 19);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(147, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "Set Up Media Drive";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(10, 47);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 134);
            this.listBox1.TabIndex = 6;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(136, 47);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Sync";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(136, 76);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 24);
            this.button8.TabIndex = 8;
            this.button8.Text = "Remove Binding";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "{Message}";
            this.notifyIcon1.BalloonTipTitle = "iTunes Helper";
            this.notifyIcon1.ContextMenuStrip = this.notifyIconContextMenuStrip;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "iTunes Helper";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // notifyIconContextMenuStrip
            // 
            this.notifyIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.showToolStripMenuItem,
                                    this.setUpNewToolStripMenuItem,
                                    this.exitoniTunesCloseToolStripMenuItem,
                                    this.exitITunesHelperToolStripMenuItem});
            this.notifyIconContextMenuStrip.Name = "notifyIconContextMenuStrip";
            this.notifyIconContextMenuStrip.Size = new System.Drawing.Size(178, 92);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // setUpNewToolStripMenuItem
            // 
            this.setUpNewToolStripMenuItem.Name = "setUpNewToolStripMenuItem";
            this.setUpNewToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.setUpNewToolStripMenuItem.Text = "Set up new Device";
            this.setUpNewToolStripMenuItem.Click += new System.EventHandler(this.setUpNewToolStripMenuItem_Click);
            // 
            // exitoniTunesCloseToolStripMenuItem
            // 
            this.exitoniTunesCloseToolStripMenuItem.CheckOnClick = true;
            this.exitoniTunesCloseToolStripMenuItem.Name = "exitoniTunesCloseToolStripMenuItem";
            this.exitoniTunesCloseToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.exitoniTunesCloseToolStripMenuItem.Text = "Exit on iTunes close";
            this.exitoniTunesCloseToolStripMenuItem.Click += new System.EventHandler(this.exitoniTunesCloseToolStripMenuItem_Click);
            // 
            // exitITunesHelperToolStripMenuItem
            // 
            this.exitITunesHelperToolStripMenuItem.Name = "exitITunesHelperToolStripMenuItem";
            this.exitITunesHelperToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.exitITunesHelperToolStripMenuItem.Text = "Exit iTunes Helper";
            this.exitITunesHelperToolStripMenuItem.Click += new System.EventHandler(this.ExitITunesHelperToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Location = new System.Drawing.Point(216, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 400);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Devices";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.scheduledSyncsListBox);
            this.groupBox4.Controls.Add(this.button14);
            this.groupBox4.Controls.Add(this.button13);
            this.groupBox4.Controls.Add(this.button12);
            this.groupBox4.Location = new System.Drawing.Point(10, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(218, 125);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scheduled Syncs";
            // 
            // scheduledSyncsListBox
            // 
            this.scheduledSyncsListBox.FormattingEnabled = true;
            this.scheduledSyncsListBox.Location = new System.Drawing.Point(6, 20);
            this.scheduledSyncsListBox.Name = "scheduledSyncsListBox";
            this.scheduledSyncsListBox.Size = new System.Drawing.Size(120, 95);
            this.scheduledSyncsListBox.TabIndex = 11;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(133, 79);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 14;
            this.button14.Text = "Remove";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.Button14_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(133, 50);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 13;
            this.button13.Text = "Stop";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.Button13_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(133, 21);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 12;
            this.button12.Text = "Start";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.Button12_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(137, 106);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 44);
            this.button10.TabIndex = 9;
            this.button10.Text = "Schedule Sync";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button16);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button15);
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(10, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 294);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Commands";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(19, 203);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(147, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "Get Lyrics for all songs";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(19, 173);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(147, 23);
            this.button15.TabIndex = 6;
            this.button15.Text = "Backup Library";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.Button15_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(19, 144);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(147, 23);
            this.button11.TabIndex = 5;
            this.button11.Text = "Convert Files";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.MWCCheckBox);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Location = new System.Drawing.Point(10, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accessories";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(6, 67);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(136, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Get Lyrics on song play";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // MWCCheckBox
            // 
            this.MWCCheckBox.AutoSize = true;
            this.MWCCheckBox.Checked = true;
            this.MWCCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MWCCheckBox.Location = new System.Drawing.Point(6, 48);
            this.MWCCheckBox.Name = "MWCCheckBox";
            this.MWCCheckBox.Size = new System.Drawing.Size(145, 17);
            this.MWCCheckBox.TabIndex = 1;
            this.MWCCheckBox.Text = "Minimize Instead of Close";
            this.MWCCheckBox.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(6, 19);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(187, 23);
            this.button9.TabIndex = 0;
            this.button9.Text = "OnTop Music Controller";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Button9_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                    this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 415);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(471, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(19, 233);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(147, 23);
            this.button16.TabIndex = 8;
            this.button16.Text = "Apply iTunes Data to files";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.Button16_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 437);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "iTunes Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.notifyIconContextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox MWCCheckBox;
        private System.Windows.Forms.ToolStripMenuItem exitITunesHelperToolStripMenuItem;
        internal System.Windows.Forms.Button button15;
        internal System.Windows.Forms.ListBox scheduledSyncsListBox;
        internal System.Windows.Forms.Button button12;
        internal System.Windows.Forms.Button button13;
        internal System.Windows.Forms.Button button14;
        internal System.Windows.Forms.Button button11;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button button4;
        internal System.Windows.Forms.Button button2;
        internal System.Windows.Forms.Button button3;
        internal System.Windows.Forms.Button button6;
        internal System.Windows.Forms.ListBox listBox1;
        internal System.Windows.Forms.Button button7;
        internal System.Windows.Forms.Button button8;
        internal System.Windows.Forms.NotifyIcon notifyIcon1;
        internal System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip;
        internal System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem setUpNewToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitoniTunesCloseToolStripMenuItem;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.GroupBox groupBox3;
        internal System.Windows.Forms.Button button9;
        internal System.Windows.Forms.Button button10;
    }
}
