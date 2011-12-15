using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
namespace iTunesHelper.Forms
{

	public partial class GetEncryptionCodeForm : System.Windows.Forms.Form
	{
		public string Password;

		public string EncryptingFile;
		private void OK_Button_Click(System.Object sender, System.EventArgs e)
		{
			if (TextBox1.Text != TextBox2.Text) {
				MessageBox.Show("Passwords don't match!");
				return;
			} else if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text)) {
				MessageBox.Show("Passwords are blank!");
				return;
			} else {
			}
			this.Password = TextBox1.Text;
			this.Close();
		}

		private void GetEncryptionCodeForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (TextBox1.Text != TextBox2.Text) {
				MessageBox.Show("Passwords don't match!");
				e.Cancel = true;
			} else if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text)) {
				MessageBox.Show("Passwords are blank!");
				e.Cancel = true;
			} else {
				Password = TextBox1.Text;
			}
		}

		private void GetEncryptionCodeForm_Load(System.Object sender, System.EventArgs e)
		{
			this.Text = "Encryption Code for " + EncryptingFile;
			this.Label1.Text = "Encryption Code for " + System.IO.Path.GetFileName(EncryptingFile);
		}

		public GetEncryptionCodeForm(string file)
		{
			Load += GetEncryptionCodeForm_Load;
			FormClosing += GetEncryptionCodeForm_FormClosing;
			InitializeComponent();
			this.EncryptingFile = file;
		}
	}
}
