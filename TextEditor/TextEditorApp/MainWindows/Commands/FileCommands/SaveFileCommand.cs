﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class SaveFileCommand : ICommand
    {
        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067
        protected readonly MainWinViewModel CallerViewModel;

        public SaveFileCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (CallerViewModel.ActiveTextEditor == null || CallerViewModel.ActiveTextEditor.DocumentModel.IsSaved == true)
            {
                return;
            }
            else if(CallerViewModel.ActiveTextEditor != null && CallerViewModel.ActiveTextEditor.Text == string.Empty)
            {
                return;
            }
            else if(CallerViewModel.ActiveTextEditor != null && File.Exists(CallerViewModel.ActiveTextEditor.DocumentModel.FilePath))
            {
                string filePath = CallerViewModel.ActiveTextEditor.DocumentModel.FilePath;
                SaveContentToFile(filePath);
            }
            else
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                {
                    Filter = "All files|*.*",
                    DefaultExt = ".txt"
                };

                if (saveFileDialog.ShowDialog() == true && CallerViewModel.ActiveTextEditor != null)
                {
                    var filepath = saveFileDialog.FileName;
                    CallerViewModel.ActiveTextEditor.DocumentModel.FilePath = filepath;
                    CallerViewModel.ActiveTextEditor.DocumentModel.FileName = Path.GetFileName(filepath);
                    SaveContentToFile(filepath);
                }
            }
        }

        private void SaveContentToFile(string filepath)
        {
            if (CallerViewModel.ActiveTextEditor != null)
            {
                try
                {
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
