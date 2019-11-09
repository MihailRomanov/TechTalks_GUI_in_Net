using System;
using Eto.Forms;
using Eto.Drawing;

namespace EtoApp1.Desktop
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			new Application(Eto.Platforms.Gtk).Run(new MainForm());
		}
	}
}