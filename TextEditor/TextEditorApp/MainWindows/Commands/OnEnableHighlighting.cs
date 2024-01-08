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

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnEnableHighlighting : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnEnableHighlighting(MainWinViewModel callerViewModel)
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
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");
                }
            }
        }
    }
}
