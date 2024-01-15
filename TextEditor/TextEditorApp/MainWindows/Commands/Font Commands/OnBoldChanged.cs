using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;
using TextEditorApp.MainWindows.WinViewModels;
using Xceed.Wpf.Toolkit;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnBoldChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnBoldChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public OnBoldChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the bold change command.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && CallerViewModel.ActiveTextEditor != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                if (toggleButton.IsChecked == true)
                {
                    textEditor.FontWeight = FontWeights.Bold;
                }
                else
                {
                    textEditor.FontWeight = FontWeights.Normal;
                }
            }
        }
    }
}
