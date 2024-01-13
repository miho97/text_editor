using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;
using System.Drawing;
using System.Drawing.Printing;
using RoslynPad.Editor;

namespace TextEditorApp.MainWindows.Commands
{
    internal class PrintCommand : ICommand
    {
        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067
        protected readonly MainWinViewModel CallerViewModel;

        public PrintCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
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
                var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += (sender, e) =>
                    {
                        // TODO - check printing, bind with corresponding font and size
                        e?.Graphics?.DrawString(textEditor.Text, new Font("Arial", 10), Brushes.Black, 10, 10);
                    };                    
                    try
                    {
                        
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
}
