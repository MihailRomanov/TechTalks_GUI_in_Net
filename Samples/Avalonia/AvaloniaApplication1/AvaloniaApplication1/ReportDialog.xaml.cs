using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;
using FormGenerator.Models;

namespace AvaloniaApplication1
{
    public class ReportDialog : Window
    {
        public ReportDialog(MeetingNotes meetingNotes) : this()
        {
            DataContext = new ReportDialogViewModel(this, meetingNotes);
        }

        public ReportDialog()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
