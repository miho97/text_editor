using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;
using Xceed.Wpf.Toolkit;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for handling font color changes in the text editor.
    /// </summary>
    internal class OnFontColorChange : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnFontColorChange"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public OnFontColorChange(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the font color change command.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // Selected color ili SelectedcolorTExt
            if (parameter is RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> args && args.Source is ColorPicker picker  && picker.SelectedColorText is string selCol && selCol != null)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                var converter = new System.Windows.Media.BrushConverter();

                // over the top null-check because of non stoping complier warnings
                selCol ??= "FFFFFFFF";
                var conversion = converter.ConvertFromString(selCol);
                textEditor.Foreground = (conversion != null) ? (Brush)conversion : Brushes.Black;
            }
        }
    }
}
