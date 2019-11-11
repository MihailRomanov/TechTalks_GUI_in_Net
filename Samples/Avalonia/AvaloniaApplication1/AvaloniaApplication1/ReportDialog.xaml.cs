using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;
using FormGenerator.Models;

namespace AvaloniaApplication1
{
    public class ReportDialog : Window
    {
        private ReportDialogViewModel viewModel;
        public ReportDialog(MeetingNotes meetingNotes)
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            viewModel = new ReportDialogViewModel(this, meetingNotes);
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
