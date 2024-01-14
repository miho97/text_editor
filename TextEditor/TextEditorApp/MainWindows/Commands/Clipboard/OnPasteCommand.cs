using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnPasteCommand : BaseCommandClass
    {
        public OnPasteCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }
 
        public override void Execute(object? parameter)
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    if (Clipboard.ContainsText())
                    {
                        try { textEditor.Paste(); }
                        catch (Exception)
                        {
                            MessageBox.Show($"Error in pasting to file");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You don't have anything in your clipboard.", "Clipboard");
                    }
                }
            }
        }
    }
}
