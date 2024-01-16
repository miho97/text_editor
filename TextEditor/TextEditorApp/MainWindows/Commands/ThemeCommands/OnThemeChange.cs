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
    /// <summary>
    /// Command for theme changes in the text editor.
    /// </summary>
    internal class OnThemeChange : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnThemeChange"/> class.
        /// </summary>
        /// <param name="callerViewModel">The MainWinViewModel associated with the command.</param>
        public OnThemeChange(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command logic when switching between visual themes.
        /// </summary>
        /// 

        // we request from ThemeManager to change the visual theme, and then Main window must execute some needed logic
        public override void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    // Themes are in separete Project so we are using packing
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
