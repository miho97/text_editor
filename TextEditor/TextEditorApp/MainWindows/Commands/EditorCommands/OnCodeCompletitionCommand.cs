using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Controls.ControlsModels;
using System.Windows;
using TextEditorApp.Intellisense.Service;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnCodeCompletitionCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        protected readonly MainWinViewModel CallerViewModel;

        public OnCodeCompletitionCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            var roslynHandler = new RoslynCodeEditorHandler();
                            await roslynHandler.InitializeRoslynCodeEditorAsync(textEditor);
                        }
                    }
                }
                else
                {
                    if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            textEditor.CompletionProvider = null;
                        }
                    }
                }
            }
        }
    }
}
