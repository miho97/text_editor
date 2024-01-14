using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling the Paste command, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnPasteCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnPasteCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnPasteCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the Paste command, pasting the content from the clipboard to the active editor..
        /// </summary>
        public override void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (textEditor != null)
            {
                if (Clipboard.ContainsText())
                {
                    try { textEditor.Paste(); }
                    catch (Exception)
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
