using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnCopyCommand : BaseCommandClass
    {
        public OnCopyCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    try { textEditor.Copy(); }
                    catch (Exception)
                    {
                        MessageBox.Show($"Error in copying from file");
                    }
                }
            }
        }
    }
}
