using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows;
using System.Windows.Input;
using TextEditorApp.Intellisense.Service;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnPrimCodeCompletitionCommand : ICommand
    {
        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067
        protected readonly MainWinViewModel CallerViewModel;

        public OnPrimCodeCompletitionCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && textEditor != null)
            {
                textEditor.IsPrimIntellisenseEnabled = toggleButton.IsChecked ?? false;
            }
        }
    }
}
