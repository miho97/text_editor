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
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "All files|*.*",
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filename = saveFileDialog.FileName;
                SaveContentToFile(filename);
            }
        }

        private void SaveContentToFile(string filename)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    try
                    {
                        File.WriteAllText(filename, textEditor.Text);

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error in saving file");
                    }
                }
            }
        }
    }
}
