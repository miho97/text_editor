using System;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.IO;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OpenFileCommand : BaseCommandClass
    {
        public OpenFileCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                OpenFile(filename);
            }
        }

        private void OpenFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return;
            }

            CallerViewModel.NewFileCommand.Execute(CallerViewModel);

            if (CallerViewModel.ActiveTextEditor != null)
            {
                try
                {
                    var fileContent = File.ReadAllText(filepath);
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
