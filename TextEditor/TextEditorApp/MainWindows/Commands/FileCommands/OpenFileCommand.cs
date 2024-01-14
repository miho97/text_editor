using System;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.IO;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for opening a file and loading its content into the active text editor.
    /// </summary>
    internal class OpenFileCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public OpenFileCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to open a file.
        /// </summary>
        public override void Execute(object? parameter)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Display the open file dialog
            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                OpenFile(filename);
            }
        }

        /// <summary>
        /// Opens the specified file and loads its content into the active text editor.
        /// </summary>
        /// <param name="filepath">The path of the file to be opened.</param>
        private void OpenFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return;
            }

            // Create a new file using the NewFileCommand - this way we avoid code duplication since either way we want to open the document in the new tab 
            CallerViewModel.NewFileCommand.Execute(CallerViewModel);

            // Load the file content into the active text editor
            if (CallerViewModel.ActiveTextEditor != null)
            {
                try
                {
                    // Read the content of the file
                    var fileContent = File.ReadAllText(filepath);

                    // Set the text editor properties
                    CallerViewModel.ActiveTextEditor.Text = fileContent;
                    CallerViewModel.ActiveTextEditor.DocumentModel.Content = fileContent;
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
