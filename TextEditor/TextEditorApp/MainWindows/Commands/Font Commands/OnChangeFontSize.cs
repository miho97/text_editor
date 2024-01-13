using RoslynPad.Editor;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnChangeFontSize : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnChangeFontSize(MainWinViewModel callerViewModel)
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
            if (parameter is SelectionChangedEventArgs args && args.AddedItems[0] is FontSizeListModel fModel 
                && fModel.FontSize is double fontSize)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        textEditor.FontSize = fontSize;
                        CallerViewModel.SelectedFontSize.FontSize = fontSize;
                    }
                }
            }
            else if(parameter is TextCompositionEventArgs args2 && args2.Text is string fontSizeText)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        if (int.TryParse(fontSizeText, out int fontSizeTextToInt))
                        {
                            textEditor.FontSize = fontSizeTextToInt;
                            CallerViewModel.SelectedFontSize.FontSize = fontSizeTextToInt;
                        }
                        else
                        {
                            Console.WriteLine("Nije moguće konvertirati string u double.");
                        }
                    }
                }
            }
            else if(parameter is String fontSizeString)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        if (int.TryParse(fontSizeString, out int fontSizeTextToInt))
                        {
                            textEditor.FontSize = fontSizeTextToInt > 0 ? fontSizeTextToInt : 15;
                            CallerViewModel.SelectedFontSize.FontSize = fontSizeTextToInt;
                        }
                        else
                        {
                            Console.WriteLine("Nije moguće konvertirati string u double.");
                        }
                    }
                }
            }
        }
    }
}
