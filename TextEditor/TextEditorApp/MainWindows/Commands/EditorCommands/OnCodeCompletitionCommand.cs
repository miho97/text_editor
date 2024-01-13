using System;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using TextEditorApp.Intellisense.Service;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnCodeCompletitionCommand : ICommand
    {
        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067
        protected readonly MainWinViewModel CallerViewModel;

        public OnCodeCompletitionCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && textEditor != null)
            {
                if (toggleButton.IsChecked == true)
                {
                    var roslynHandler = new RoslynCodeEditorHandler();
                    await roslynHandler.InitializeRoslynCodeEditorAsync(textEditor);
                }
                else
                {
                    textEditor.CompletionProvider = null;
                }
            }
        }
    }
}
