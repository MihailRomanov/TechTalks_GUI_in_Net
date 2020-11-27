using Common;
using FormGenerator.Models;
using Gtk;
using System;

namespace App1
{
    internal class MainWindow : Window
    {
        MeetingNotes meetingNotes = null;

        public MainWindow() : base(WindowType.Toplevel)
        {
            Initialize();
            DeleteEvent += MainWindow_DeleteEvent;

            ShowAll();
        }

        private void Initialize()
        {
            DefaultHeight = 200;
            DefaultWidth = 300;
            MenuBar menuBar = CreateMenuBar();

            var vbox = new VBox() { Visible = true };
            vbox.PackStart(menuBar, false, false, 0);

            var frame = new TextView() { Visible = true };
            vbox.PackStart(frame, true, true, 0);

            this.Add(vbox);
        }

        private MenuBar CreateMenuBar()
        {
            var ag = new AccelGroup();
            this.AddAccelGroup(ag);

            var fileMenu = new Menu();
            ImageMenuItem newMenuItem = new ImageMenuItem(Stock.New, ag);
            newMenuItem.Activated += NewMenuItem_Activated;
            fileMenu.Append(newMenuItem);
            ImageMenuItem openMenuItem = new ImageMenuItem(Stock.Open, ag);
            openMenuItem.Activated += OpenMenuItem_Activated;
            fileMenu.Append(openMenuItem);
            fileMenu.Append(new SeparatorMenuItem());
            ImageMenuItem quitMenuItem = new ImageMenuItem(Stock.Quit, ag);
            quitMenuItem.Activated += QuitMenuItem_Activated;
            fileMenu.Append(quitMenuItem);

            var menu = new MenuBar();
            menu.Append(new ImageMenuItem(Stock.File, null) { Submenu = fileMenu });
            ImageMenuItem generateMenuItem = new ImageMenuItem("Generate");
            generateMenuItem.Activated += GenerateMenuItem_Activated;
            menu.Append(generateMenuItem);
            menu.Append(new ImageMenuItem(Stock.About, ag));
            return menu;
        }

        private void GenerateMenuItem_Activated(object sender, EventArgs e)
        {
            if (meetingNotes == null)
                return;

            var saveFileDialog = new FileChooserDialog("Save report as ...", this, FileChooserAction.Save,
                "OK", ResponseType.Ok, "Cancel", ResponseType.Cancel);
            saveFileDialog.SetFilename("report.docx");

            var dialogResult = saveFileDialog.Run();
            saveFileDialog.Hide();

            if (dialogResult != (int)ResponseType.Ok)
                return;

            var filename = saveFileDialog.Filename;

            GenerationHelper.GenerateAndOpen(filename, meetingNotes);
        }

        private void OpenMenuItem_Activated(object sender, EventArgs e)
        {
            using var dialog = new MeetingNotesDialog(this, meetingNotes ?? new MeetingNotes());
            var result = dialog.Run();
            if (result == (int)ResponseType.Ok)
                meetingNotes = dialog.ToMeetingNotes();
        }

        private void QuitMenuItem_Activated(object sender, EventArgs e)
        {
            Application.Quit();
        }

        private void NewMenuItem_Activated(object sender, EventArgs e)
        {
            using var dialog = new MeetingNotesDialog(this, new MeetingNotes());
            var result = dialog.Run();
            if (result == (int)ResponseType.Ok)
                meetingNotes = dialog.ToMeetingNotes();
        }

        private void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
        {
            Application.Quit();
        }
    }
}
