using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditorApp.MainWindows.WinViewModels;
using System.IO;
using System.Windows;
using ICSharpCode.AvalonEdit;
using System.Windows.Controls;
using System.Windows.Input;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnRemoveTabCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnRemoveTabCommand(MainWinViewModel callerViewModel)
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
            if (parameter is MouseEventArgs args && args.Source is TabItem tabItem && args.RightButton == MouseButtonState.Pressed)
            {
                CallerViewModel.MainTabControl.Items.Remove(tabItem);
                CallerViewModel.FileCount--;

                if(CallerViewModel != null && CallerViewModel.FileCount <= 0)
                {
                    CallerViewModel.NewFileCommand.Execute(CallerViewModel);
                }
            }
        }
    }
}
