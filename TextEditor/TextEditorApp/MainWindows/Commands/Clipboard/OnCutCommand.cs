using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling the Cut command, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnCutCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnCutCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnCutCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }


        /// <summary>
        /// Executes the Cut command, cutting the selected content from the active text editor.
        /// If nothing is selected the whole text from the editor will be cut.
        /// </summary>
        public override void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (textEditor != null)
            {
                try { textEditor.Cut(); }
                catch (Exception)
                {
                    MessageBox.Show($"Error in cutting from file");
                }
            }
        }
    }
}
