using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace EtoApp1
{ 
    public class MainForm : Form
    { 
        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void CreateNew(object sender, EventArgs e)
        {
            var dialog = new ReportDialog();
            dialog.ShowModal();
        }

        protected void Generate(object sender, EventArgs e)
        {
            MessageBox.Show("Generated!");
        }

        protected void HandleAbout(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}
