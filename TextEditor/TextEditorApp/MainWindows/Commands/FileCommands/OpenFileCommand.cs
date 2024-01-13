﻿using System;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.IO;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OpenFileCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OpenFileCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
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
