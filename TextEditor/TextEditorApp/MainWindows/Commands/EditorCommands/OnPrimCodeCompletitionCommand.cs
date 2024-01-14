using System.Windows.Controls.Ribbon;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnPrimCodeCompletitionCommand : BaseCommandClass
    {
        public OnPrimCodeCompletitionCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && textEditor != null)
            {
                textEditor.IsPrimIntellisenseEnabled = toggleButton.IsChecked ?? false;
            }
        }
    }
}
