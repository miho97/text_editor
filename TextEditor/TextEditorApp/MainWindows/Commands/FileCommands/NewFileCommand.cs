using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class NewFileCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public NewFileCommand(MainWinViewModel callerViewModel)
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
            CallerViewModel.FileCount++;
            TabItem newTab = new TabItem();

            newTab.PreviewMouseRightButtonDown += (sender, args) =>
            {
                CallerViewModel.OnRemoveTabCommand?.Execute(args);
            };

            DockPanel panel = new DockPanel();

            TextBlock statusBar = new TextBlock();
            
            DockPanel.SetDock(statusBar, Dock.Bottom);

            var textEditor = new CustomTextEditorModel();
            DockPanel.SetDock(textEditor, Dock.Top);

            panel.Children.Add(statusBar);
            panel.Children.Add(textEditor);

            newTab.Content = panel;

            textEditor.DockParent = panel;

            var binding = new Binding(nameof(textEditor.DocumentModel.FileName));
            binding.Source = textEditor.DocumentModel;

            newTab.SetBinding(HeaderedContentControl.HeaderProperty, binding);


            statusBar.Text = "Status bar for " + newTab.Header;

            CallerViewModel.MainTabControl.Items.Add(newTab);

            CallerViewModel.MainTabControl.SelectedItem = newTab;

            CallerViewModel.ActiveTextEditor = textEditor;

            textEditor.Focus();

        }
    }
}
