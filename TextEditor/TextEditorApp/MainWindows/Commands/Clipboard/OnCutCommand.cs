using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnCutCommand : BaseCommandClass
    {

        public OnCutCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }


        public override void Execute(object? parameter)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    {
                        try { textEditor.Cut(); }
                        catch (Exception)
                        {
                            MessageBox.Show($"Error in cutting from file");
                        }
                    }
                }
            }
        }
    }
}
