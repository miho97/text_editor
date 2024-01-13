using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnPasteCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnPasteCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    if (Clipboard.ContainsText())
                    {
                        try { textEditor.Paste(); }
                        catch (Exception)
                        {
                            MessageBox.Show($"Error in pasting to file");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You don't have anything in your clipboard.", "Clipboard");
                    }
                }
            }
        }
    }
}
