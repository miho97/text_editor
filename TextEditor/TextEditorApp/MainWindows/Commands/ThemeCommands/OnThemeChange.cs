using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using System.Windows.Controls.Ribbon;
using Elfie.Serialization;

namespace TextEditorApp.MainWindows.Commands
{
    public class OnThemeChange : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnThemeChange(MainWinViewModel callerViewModel)
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
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {

                    ThemeManager.ChangeTheme(new Uri("/TextEditorApp;component/MainWindows/Commands/ThemeCommands/DarkTheme.xaml", UriKind.RelativeOrAbsolute));
                    CallerViewModel.IsThemeChangeEnabled = true;

                }
                else
                {
                    ThemeManager.ChangeTheme(new Uri("/TextEditorApp;component/MainWindows/Commands/ThemeCommands/LightTheme.xaml", UriKind.RelativeOrAbsolute));
                    CallerViewModel.IsThemeChangeEnabled = false;
                }

            
            }
                  
        }
    }
}
