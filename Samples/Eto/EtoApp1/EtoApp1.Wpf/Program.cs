using System;
using Eto;
using Eto.Forms;

namespace EtoApp1.Wpf
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			new Application(Platforms.Wpf).Run(new MainForm());
		}
	}
}