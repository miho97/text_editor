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
using System.Windows;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnShowLineNumbersChanged : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnShowLineNumbersChanged(MainWinViewModel callerViewModel)
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

        public void Execute(object? parametar)
        {
            if (parametar is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            textEditor.ShowLineNumbers = true;
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
                            textEditor.ShowLineNumbers = false;
                        }
                    }
                }
            }
        }

    }
}
