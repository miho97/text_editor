using System.Windows.Controls.Ribbon;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling changes in the primary code completion feature, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnPrimCodeCompletitionCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnPrimCodeCompletitionCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnPrimCodeCompletitionCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to toggle the primary code completion feature in the active text editor.
        /// </summary>
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
