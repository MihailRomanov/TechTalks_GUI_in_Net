using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace EtoApp1
{	
	public class ReportDialog : Dialog
	{	
		public ReportDialog()
		{
			XamlReader.Load(this);
		}
	}
}
