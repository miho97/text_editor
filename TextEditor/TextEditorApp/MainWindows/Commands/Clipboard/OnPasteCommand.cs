using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public event EventHandler? CanExecuteChanged;

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
                        catch (Exception e)
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
