using System;
using System.IO;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    internal class SaveFileAsCommand : BaseCommandClass
    {
        public SaveFileAsCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            if(CallerViewModel.ActiveTextEditor == null )
            {
                return;
            }

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
