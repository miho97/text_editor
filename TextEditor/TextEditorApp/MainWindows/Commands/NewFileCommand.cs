using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TextEditorApp.MainWindows.Commands
{
    internal class NewFileCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public NewFileCommand(MainWinViewModel callerViewModel)
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
            // tabItem function called on every raise of event Preview...
            // += for possible list of events
            TabItem newTab = new TabItem();
            newTab.Header = "Untitled" + (++CallerViewModel.FileCount) + ".txt";

            newTab.PreviewMouseRightButtonDown += (sender, args) =>
            {
                // Ovdje smo u kontekstu gdje je args dostupan
                CallerViewModel.OnRemoveTabCommand?.Execute(args);
            };

            DockPanel panel = new DockPanel();

            TextBlock statusBar = new TextBlock();
            statusBar.Text = "Status bar for " + newTab.Header;
            DockPanel.SetDock(statusBar, Dock.Bottom);

            var textEditor = new CustomTextEditorModel();
            DockPanel.SetDock(textEditor, Dock.Top);

            panel.Children.Add(statusBar);
            panel.Children.Add(textEditor);

            newTab.Content = panel;

            CallerViewModel.MainTabControl.Items.Add(newTab);

            CallerViewModel.MainTabControl.SelectedItem = newTab;

            textEditor.Focus();
        }
    }
}
