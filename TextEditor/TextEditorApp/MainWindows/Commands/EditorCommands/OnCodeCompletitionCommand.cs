using System.Windows.Controls.Ribbon;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using TextEditorApp.Intellisense.Service;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnCodeCompletitionCommand : BaseCommandClass
    {
        public OnCodeCompletitionCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override async void Execute(object? parameter)
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
