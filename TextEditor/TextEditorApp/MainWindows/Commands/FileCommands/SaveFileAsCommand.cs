using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using RoslynPad.Editor;

namespace TextEditorApp.MainWindows.Commands
{
    internal class SaveFileAsCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        protected readonly MainWinViewModel CallerViewModel;

        public SaveFileAsCommand(MainWinViewModel callerViewModel) 
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
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
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error in saving file");
                }
            }
        }
    }
}
