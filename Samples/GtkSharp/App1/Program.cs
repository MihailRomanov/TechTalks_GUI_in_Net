using Gtk;
using System;

namespace App1
{
    class Program
    {
        static void Main()
        {
            Application.Init();

            var app = new Application("org.App1", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var mainWindow = new MainWindow();
            app.AddWindow(mainWindow);

            Application.Run();
        }
    }
}
