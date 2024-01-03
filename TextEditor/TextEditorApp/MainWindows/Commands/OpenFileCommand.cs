using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        public event EventHandler? CanExecuteChanged;

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

        private void OpenFile(string filename)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    try
                    {
                        textEditor.Text = File.ReadAllText(filename);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error in saving file");
                    }
                }
            }
        }
    }
}
