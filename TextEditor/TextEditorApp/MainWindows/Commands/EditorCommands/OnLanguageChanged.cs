using ICSharpCode.AvalonEdit.Highlighting;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling changes in the selected programming language, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnLanguageChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnLanguageChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnLanguageChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to handle changes in the selected programming language.
        /// </summary>
        /// 

        // sets the newly selected programming language as a syntax highlighting standars, also saves the language to document model
        public override void Execute(object? parameter)
        {
            if (parameter is RoutedEventArgs args && args.Source is ComboBox languageCombobox && CallerViewModel.ActiveTextEditor != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                var SelectedLanguage = languageCombobox.SelectedItem as string;

                if(SelectedLanguage != null)
                {
                    textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(SelectedLanguage);
                    textEditor.DocumentModel.DocumentLanguage = new LanguageViewModel(SelectedLanguage, true);
                }
            }
        }
    }
}
