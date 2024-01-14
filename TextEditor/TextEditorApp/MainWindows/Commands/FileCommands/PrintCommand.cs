using System;
using System.Linq;
using System.Windows.Controls;
using TextEditorApp.MainWindows.WinViewModels;
using System.Drawing;
using System.Drawing.Printing;
using RoslynPad.Editor;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for printing the content of the active text editor.
    /// </summary>
    internal class PrintCommand : BaseCommandClass
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model that owns this command.</param>
        public PrintCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to print the content of the active text editor.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // Show print dialog to select print settings.
            PrintDialog pDialog = new PrintDialog();
            pDialog.MaxPage = 1;
            pDialog.UserPageRangeEnabled = false;
            pDialog.SelectedPagesEnabled = false;

            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                PrintContent();
                return;
            }
        }

        private void PrintContent()
        {
            if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = CallerViewModel.ActiveTextEditor;
                var docModel = textEditor.DocumentModel;

                // Create a PrintDocument and handle the PrintPage event for custom printing.
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (sender, e) =>
                {
                    // TODO - check printing
                    e?.Graphics?.DrawString(textEditor.Text, new Font(docModel.DocumentFontFamily.FontfamilyString, (float)docModel.FontSize.FontSize), Brushes.Black, 10, 10);
                };                    
                try
                {
                    pd.Print();
                }
                catch (Exception)
                {
                    // TODO
                    return;
                }
            }
        }
    }
}
