using System.Linq;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling changes in the visibility of line numbers, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnShowLineNumbersChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnShowLineNumbersChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnShowLineNumbersChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to change the visibility of line numbers in the active text editor.
        /// </summary>
        public override void Execute(object? parametar)
        {
            if (parametar is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && CallerViewModel.ActiveTextEditor != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                if (toggleButton.IsChecked == true)
                {
                    textEditor.ShowLineNumbers = true;
                }
                else
                {
                    textEditor.ShowLineNumbers = false;
                }
            }
        }
    }
}
