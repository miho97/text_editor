using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for handling cursive family changes in the text editor.
    /// </summary>
    internal class OnItalicChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnItalicChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public OnItalicChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the italic change command.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && CallerViewModel.ActiveTextEditor != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                if (toggleButton.IsChecked == true)
                {
                    textEditor.FontStyle = FontStyles.Italic;
                }
                else
                {
                    textEditor.FontStyle = FontStyles.Normal;
                }
            }
        }
    }
}
