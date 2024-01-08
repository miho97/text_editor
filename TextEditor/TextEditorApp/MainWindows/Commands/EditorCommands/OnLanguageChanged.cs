using ICSharpCode.AvalonEdit.Highlighting;
using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnLanguageChanged : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnLanguageChanged(MainWinViewModel callerViewModel)
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
            if (parameter is RoutedEventArgs args && args.Source is ComboBox languageCombobox)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var SelectedLanguage = languageCombobox.SelectedItem as string;
                    var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                    if (textEditor != null && SelectedLanguage != null)
                    {
                        textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(SelectedLanguage);
                        textEditor.DocumentModel.DocumentLanguage = new LanguageViewModel(SelectedLanguage, true);
                    }
                }
            }
        }
    }
}
