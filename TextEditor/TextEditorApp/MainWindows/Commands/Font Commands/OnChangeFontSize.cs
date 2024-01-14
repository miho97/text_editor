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
    /// <summary>
    /// Command for changing the font size in the active text editor.
    /// </summary>
    internal class OnChangeFontSize : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnChangeFontSize"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model that owns this command.</param>
        public OnChangeFontSize(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to change the font size.
        /// </summary>

        // we are setting font size to recieved value
        // since font can be set from multiple sources we have this clunky if-else chunk

        // TODO - ugly code- try to refactor
        public override void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (textEditor == null) return; // return early

            if (parameter is SelectionChangedEventArgs args && args.AddedItems[0] is FontSizeListModel fModel 
                && fModel.FontSize is double fontSize)
            {
                        textEditor.FontSize = fontSize;
                        CallerViewModel.SelectedFontSize.FontSize = fontSize;
            }
            else if (parameter is TextCompositionEventArgs args2 && args2.Text is string fontSizeText)
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
            else if (parameter is String fontSizeString)
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
