using System.Linq;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnShowLineNumbersChanged : BaseCommandClass
    {
        public OnShowLineNumbersChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parametar)
        {
            if (parametar is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            textEditor.ShowLineNumbers = true;
                        }
                    }
                }
                else
                {
                    if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            textEditor.ShowLineNumbers = false;
                        }
                    }
                }
            }
        }

    }
}
