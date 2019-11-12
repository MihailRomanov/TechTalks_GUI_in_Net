using System;
using Eto;
using Eto.Forms;

namespace EtoApp1.Gtk
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			new Application(Platforms.Gtk).Run(new MainForm());
		}
	}
}