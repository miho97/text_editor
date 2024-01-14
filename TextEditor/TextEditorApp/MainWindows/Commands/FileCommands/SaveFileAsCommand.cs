using System;
using System.IO;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for saving the content of the active text editor to a new file.
    /// </summary>
    internal class SaveFileAsCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileAsCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model that owns this command.</param>
        public SaveFileAsCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to save the content of the active text editor to a new file.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if(CallerViewModel.ActiveTextEditor == null )
            {
                return;
            }

            // Show save file dialog to select a new file path.
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "All files|*.*",
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filepath = saveFileDialog.FileName;
                SaveContentToFile(filepath);
            }
        }

        private void SaveContentToFile(string filepath)
        {
            if (CallerViewModel.ActiveTextEditor != null)
            {
                try
                {
                    // Save the content to the specified file.
                    // Set filename and file path to model
                    // Set saved flag to true
                    File.WriteAllText(filepath, CallerViewModel.ActiveTextEditor.Text);
                    CallerViewModel.ActiveTextEditor.DocumentModel.FilePath = filepath;
                    CallerViewModel.ActiveTextEditor.DocumentModel.FileName = Path.GetFileName(filepath);
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
