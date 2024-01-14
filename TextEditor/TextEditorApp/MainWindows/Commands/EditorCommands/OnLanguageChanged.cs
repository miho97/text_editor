using ICSharpCode.AvalonEdit.Highlighting;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnLanguageChanged : BaseCommandClass
    {
        public OnLanguageChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }


        public override void Execute(object? parameter)
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
