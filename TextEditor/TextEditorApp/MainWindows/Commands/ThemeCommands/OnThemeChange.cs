using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.ThemeHandling;
using System.Drawing;
using Brushes = System.Windows.Media.Brushes;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnThemeChange : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnThemeChange"/> class.
        /// </summary>
        /// <param name="callerViewModel">The MainWinViewModel associated with the command.</param>
        public OnThemeChange(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {

                    ThemeManager.ChangeTheme(new Uri("pack://application:,,,/TextEditorApp.Utils;component/ThemeHandling/Theme/DarkTheme.xaml", UriKind.RelativeOrAbsolute));
                    CallerViewModel.IsThemeChangeEnabled = true;
                    CallerViewModel.UpdateWindow();

                }
                else
                {
                    ThemeManager.ChangeTheme(new Uri("pack://application:,,,/TextEditorApp.Utils;component/ThemeHandling/Theme/LightTheme.xaml", UriKind.RelativeOrAbsolute));
                    CallerViewModel.IsThemeChangeEnabled = false;
                    CallerViewModel.UpdateWindow();
                }


            }

        }
    }
}
