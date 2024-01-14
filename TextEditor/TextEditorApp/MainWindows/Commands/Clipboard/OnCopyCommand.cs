using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling the Copy command, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnCopyCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnCopyCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnCopyCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }


        /// <summary>
        /// Executes the Copy command, copying the selected content from the active text editor.
        /// /// If nothing is selected the whole text from the editor will be copied.
        /// </summary>
        public override void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (textEditor != null)
            {
                try { textEditor.Copy(); }
                catch (Exception)
                {
                    MessageBox.Show($"Error in copying from file");
                }
            }
        }
    }
}
