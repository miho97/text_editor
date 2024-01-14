using System;
using System.IO;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for saving the content of the active text editor to a file.
    /// </summary>
    internal class SaveFileCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model that owns this command.</param>
        public SaveFileCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to save the content of the active text editor to a file.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // we want to return early if the file is emply or already saved
            if (CallerViewModel.ActiveTextEditor == null || CallerViewModel.ActiveTextEditor.DocumentModel.IsSaved == true
                || CallerViewModel.ActiveTextEditor.Text == string.Empty)
            {
                return;
            }
            // if the file already exists on  the user's disc e.g. if was opened we want to save directly to that file
            else if(CallerViewModel.ActiveTextEditor != null && File.Exists(CallerViewModel.ActiveTextEditor.DocumentModel.FilePath))
            {
                string filePath = CallerViewModel.ActiveTextEditor.DocumentModel.FilePath;
                SaveContentToFile(filePath);
            }
            // if the file does not exist on the disc we copy the behaviour of save as... so thaht command should take over
            else
            {
                CallerViewModel.SaveFileAsCommand.Execute(CallerViewModel);
            }
        }

        private void SaveContentToFile(string filepath)
        {
            if (CallerViewModel.ActiveTextEditor != null)
            {
                try
                {
                    // we save the text to file and set the saved flag to true
                    File.WriteAllText(filepath, CallerViewModel.ActiveTextEditor.Text);
                    CallerViewModel.ActiveTextEditor.DocumentModel.IsSaved = true;
                }
                catch (Exception)
                {
                    MessageBox.Show($"Error in saving file");
                }
            }
        }
    }
}
