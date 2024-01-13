using System;
using System.Linq;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.Utils.DocumentFiles;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnRemoveTabCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnRemoveTabCommand(MainWinViewModel callerViewModel)
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
            if (parameter is MouseEventArgs args && args.Source is TabItem tabItem && args.RightButton == MouseButtonState.Pressed && tabItem.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null && textEditor.DocumentModel.IsSaved == false && textEditor.Text != string.Empty)
                {
                    if (!SaveOptionOnCloseEditorTab(textEditor.DocumentModel)) return;
                }

                CallerViewModel.MainTabControl.Items.Remove(tabItem);
                CallerViewModel.FileCount--;

                if(CallerViewModel != null && CallerViewModel.FileCount <= 0)
                {
                    CallerViewModel.NewFileCommand.Execute(CallerViewModel);
                }
            }
        }

        private bool SaveOptionOnCloseEditorTab(DocumentFiles_Model docModel)
        {
            MessageBoxResult result = MessageBox.Show("Save file " + docModel.FileName, "Save?", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CallerViewModel.SaveFileCommand.Execute(CallerViewModel);
                    return true;
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}
