using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnChangeFontSize : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnChangeFontSize(MainWinViewModel callerViewModel)
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
            if(parameter is SelectionChangedEventArgs args && args.AddedItems[0] is int fontSize)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        textEditor.FontSize = fontSize;
                    }
                }
            }
        }
    }
}
