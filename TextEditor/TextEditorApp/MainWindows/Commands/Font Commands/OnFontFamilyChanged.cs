using ICSharpCode.AvalonEdit;
using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnFontFamilyChanged : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnFontFamilyChanged(MainWinViewModel callerViewModel)
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
            if (parameter is SelectionChangedEventArgs args && args.Source is ComboBox combo && combo.SelectedValue is FontFamilyModel fontFamily)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        textEditor.FontFamily = fontFamily.Fontfamily;
                    }
                }
            }
        }
    }
}
