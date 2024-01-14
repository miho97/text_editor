using RoslynPad.Editor;
using System.Linq;
using System.Windows.Controls;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for handling font family changes in the text editor.
    /// </summary>
    internal class OnFontFamilyChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnFontFamilyChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public OnFontFamilyChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the font family change command.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if (parameter is SelectionChangedEventArgs args && args.Source is ComboBox combo && combo.SelectedValue is FontFamilyModel fontFamily
                && CallerViewModel.ActiveTextEditor != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                textEditor.FontFamily = fontFamily.Fontfamily;
            }
        }
    }
}
